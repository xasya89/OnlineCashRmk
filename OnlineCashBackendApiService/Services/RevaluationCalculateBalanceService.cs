using Dapper;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Threading.Channels;

namespace OnlineCashBackendApiService.Services;

public class RevaluationCalculateBalanceService : IDisposable
{
    private record RevaluationCalculationBalanceTask(string shopDbName, int revaluationId);


    private readonly Channel<RevaluationCalculationBalanceTask> _channel;
    private readonly CancellationTokenSource _cts = new();
    private Task? _workerTask;
    private readonly ILogger<RevaluationCalculateBalanceService> _logger;
    private readonly IDbContextFactory _dbContextFactory;

    public RevaluationCalculateBalanceService(ILogger<RevaluationCalculateBalanceService> logger, IDbContextFactory dbContextFactory)
    {
        _logger= logger;
        _dbContextFactory = dbContextFactory;
        var options = new BoundedChannelOptions(20)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true
        };
        _channel = Channel.CreateBounded<RevaluationCalculationBalanceTask>(options);
        _workerTask = Task.Run(ProcessQueueAsync);
    }

    public void EnqueueTask(string shopDbName, int revaluationId) =>
        _channel.Writer.TryWrite(new (shopDbName, revaluationId));

    private async Task ProcessQueueAsync()
    {
        await foreach (var task in _channel.Reader.ReadAllAsync(_cts.Token))
        {
            await ProcessTaskAsync(task);
        }
    }

    private async Task ProcessTaskAsync(RevaluationCalculationBalanceTask task)
    {
        try
        {
            using var con = _dbContextFactory.CreateDbContext(task.shopDbName);
            var createAt = await con.QuerySingleAsync<DateTime>($"SELECT MAX(r.Create) FROM revaluations r WHERE r.id={task.revaluationId}");
            var items = await con.QueryAsync<RevaluationItem>("SELECT * FROM revaluationgoods WHERE RevaluationId=" + task.revaluationId);
            var goodIds = items.Select(x => x.GoodId);
            var balance = await con.QueryAsync<BalanceItem>(query, new { Ids = goodIds, RevaluationCreateAt = createAt });
            var balanceDict = balance.ToDictionary(x => x.GoodId);

            foreach (var item in items)
            {
                await con.ExecuteAsync("UPDATE revaluationgoods SET CountDb=@Balance WHERE id=@Id",
                    new { Id = item.Id, Balance = balanceDict[item.GoodId].Balance });
            }
                
        }
        catch (Exception ex)
        {
            _logger.LogError("Error background calc balance revaluation\n" + ex.Message);
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _workerTask?.Wait();
    }

    private const string query = @"
                with 
                curInventory as (
	                select MAX(s.Create) AS createAt, MAX(s.id) as id FROM stocktakings s
                ),
                stocktackingWithGoods AS (
	                SELECT GoodId, CountFact AS Count, Price 
                    FROM stocktakingsummarygoods 
                    WHERE StocktakingId = (SELECT id FROM curInventory LIMIT 1)
                ),
                arrivalsWithGoods as (
	                SELECT ag.GoodId, SUM(ag.Count) as Count
                    FROM arrivals a INNER JOIN arrivalgoods ag ON a.id=ag.ArrivalId
                    WHERE a.DateArrival 
		                BETWEEN (SELECT createAt FROM curInventory LIMIT 1) AND @RevaluationCreateAt
                    GROUP BY ag.GoodId
                ),
                writeofsWithGoods as (
	                SELECT wg.GoodId, SUM(wg.Count) AS Count
                    FROM writeofs w INNER JOIN writeofgoods wg ON w.Id=wg.WriteofId
                    WHERE w.DateWriteof
		                BETWEEN (SELECT createAt FROM curInventory LIMIT 1) AND @RevaluationCreateAt
                    GROUP BY wg.GoodId
                ),
                checksWithGoods as (
	                SELECT cg.GoodId, SUM(cg.Count) AS Count
                    FROM shifts s
                    INNER JOIN checksells cs ON s.id=cs.ShiftId
                    INNER JOIN checkgoods cg ON cs.Id=cg.CheckSellId
                    WHERE s.Start
		                BETWEEN (SELECT createAt FROM curInventory LIMIT 1) AND DATE_ADD(@RevaluationCreateAt, INTERVAL 1 DAY)
                    AND cs.typeSell=0
                    GROUP BY cg.GoodId
                ),
                checksReturnWithGoods as (
	                SELECT cg.GoodId, SUM(cg.Count) AS Count
                    FROM shifts s
                    INNER JOIN checksells cs ON s.id=cs.ShiftId
                    INNER JOIN checkgoods cg ON cs.Id=cg.CheckSellId
                    WHERE s.Start
		                BETWEEN (SELECT createAt FROM curInventory LIMIT 1) AND DATE_ADD(@RevaluationCreateAt, INTERVAL 1 DAY)
                    AND cs.typeSell=1
                    GROUP BY cg.GoodId
                )
                SELECT g.id AS goodId, 
                ifnull(s.count,0) + ifnull(a.count,0) - ifnull(w.count,0) - ifnull(c.count,0) + ifnull(cReturn.count,0) as balance
                FROM goods g
                LEFT JOIN stocktackingWithGoods s ON g.id=s.goodId
                LEFT JOIN arrivalsWithGoods a ON g.id=a.goodId
                LEFT JOIN writeofsWithGoods w ON g.id=w.goodId
                LEFT JOIN checksWithGoods c ON g.id=c.goodid
                LEFT JOIN checksReturnWithGoods cReturn ON g.id=cReturn.goodid
                WHERE g.id IN @Ids
                ";

    private class RevaluationItem
    {
        public int Id { get; set; }
        public int GoodId { get; set; }
        public decimal Balance { get; set; }
    }

    private class BalanceItem
    {
        public int GoodId { get; set; }
        public decimal Balance { get; set; }
    }

    /*
     ALTER TABLE `shop7`.`revaluationgoods` 
ADD COLUMN `CountDb` DECIMAL(10,2) NULL AFTER `PriceNew`;
*/
}
