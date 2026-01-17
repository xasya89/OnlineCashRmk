using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Models;
using OnlineCashTransportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace OnlineCashRmk.Services;

public interface IDocumentSenderService
{
    public Task SendDocuments();
}
public class DocumentSenderService(IHttpClientFactory httpClientFactory, IDbContextFactory<DataContext> dbContextFactory)
    : IDocumentSenderService
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    public async Task SendDocuments()
    {
        await _semaphore.WaitAsync();
        try
        {
            using var db = dbContextFactory.CreateDbContext();
            var httpClient = httpClientFactory.CreateClient(Program.HttpClientName);
            IEnumerable<DocSynch> docs = await db.DocSynches.Where(d => d.SynchStatus == false).OrderBy(d => d.Create).ToListAsync();

            foreach (var doc in docs)
            {
                if (doc.Uuid.ToString() == "00000000-0000-0000-0000-000000000000")
                    doc.Uuid = Guid.NewGuid();
                HttpRequestMessage httpRequestMessage = null;
                switch (doc.TypeDoc)
                {
                    case TypeDocs.OpenShift:
                        httpRequestMessage = await SendOpenShift(httpClient, db, doc.DocId);
                        break;
                    case TypeDocs.Buy:
                        httpRequestMessage = await SendCheckSell(httpClient, db, doc.DocId);
                        break;
                    case TypeDocs.CloseShift:
                        httpRequestMessage = await SendCloseShift(httpClient, db, doc.DocId);
                        break;
                    case TypeDocs.WriteOf:
                        httpRequestMessage = await SendWriteOf(httpClient, db, doc.DocId);
                        break;
                    case TypeDocs.Arrival:
                        httpRequestMessage = await SendArrival(httpClient, db, doc);
                        break;
                    case TypeDocs.StockTaking:
                        break;
                    case TypeDocs.StopStocktacking:
                        httpRequestMessage = await SendStocktacking(httpClient, db, doc);
                        break;
                    case TypeDocs.CashMoney:
                         httpRequestMessage = await SendCashMoney(httpClient, db, doc.DocId);
                        break;
                    case TypeDocs.Revaluation:
                        //await SendRevaluation(doc);
                        break;
                }
                httpRequestMessage.Headers.Add("X-Document-UUID", doc.Uuid.ToString());

                var response = await httpClient.SendAsync(httpRequestMessage);
                if (response.IsSuccessStatusCode)
                {
                    doc.SynchStatus = true;
                    doc.Synch = DateTime.Now;
                    await db.SaveChangesAsync();
                }
            }

            await SynchSuppliers(httpClient, db);
        }
        finally
        {
            _semaphore.Release();
        }
    }
    private async Task<HttpRequestMessage> SendOpenShift(HttpClient httpClient, DataContext db, int shiftId)
    {
        var shift = await db.Shifts.Where(s => s.Id == shiftId)
            .AsNoTracking().FirstOrDefaultAsync();
        var body = new OpenShiftTransportModel(shift.Start, shift.Uuid);
        return new HttpRequestMessage(HttpMethod.Post, "open-shift")
        {
            Content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json")
        };
        var result = await httpClient.PostAsJsonAsync("open-shift", new OpenShiftTransportModel(shift.Start, shift.Uuid));

        if (!result.IsSuccessStatusCode)
            throw new SystemException("Ошибка отправки открытия смены");
    }

    private async Task<HttpRequestMessage> SendCloseShift(HttpClient httpClient, DataContext db, int shiftId)
    {
        var shiftclose = await db.Shifts.Where(s => s.Id == shiftId)
            .AsNoTracking().FirstOrDefaultAsync();
        var body = new CloseShiftTransportModel(shiftclose.Stop.Value, shiftclose.Uuid);
        return new HttpRequestMessage(HttpMethod.Post, "close-shift")
        {
            Content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json")
        };
        var result = await httpClient.PostAsJsonAsync("close-shift", new CloseShiftTransportModel(shiftclose.Stop.Value, shiftclose.Uuid));
        if (!result.IsSuccessStatusCode)
            throw new SystemException("Ошибка отправки закрытия смены");
    }

    private async Task<HttpRequestMessage> SendWriteOf(HttpClient httpClient, DataContext db, int writeOfId)
    {
        var writeof = await db.Writeofs
                                .Include(w => w.WriteofGoods).ThenInclude(wg => wg.Good)
                                .Where(w => w.Id == writeOfId)
                                .AsNoTracking().FirstOrDefaultAsync();
        var body = new CreateWriteOfTransportModel
        {
            DateCreate = writeof.DateCreate,
            Note = writeof.Note,
            Uuid = writeof.Uuid,
            Positions = writeof.WriteofGoods.Select(x => new CreateWriteOfPositionTransportModel
            {
                Uuid = x.Good.Uuid,
                Price = x.Price,
                Count = x.Count,
            })
        };
        return new HttpRequestMessage(HttpMethod.Post, "create-writeof")
        {
            Content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json")
        };
        var result = await httpClient.PostAsJsonAsync("create-writeof", body);
        if (!result.IsSuccessStatusCode)
            throw new SystemException("Ошибка отправки");
    }

    private async Task<HttpRequestMessage> SendArrival(HttpClient httpClient, DataContext db, DocSynch docSynch)
    {
        var arrival = await db.Arrivals
            .Include(a => a.ArrivalGoods).ThenInclude(a => a.Good)
            .Where(a => a.Id == docSynch.DocId)
            .AsNoTracking().FirstOrDefaultAsync();
        var body = new CreateArrivalTransportModel
        {
            DocumentUuid = arrival.Uuid,
            Num = arrival.Num,
            DateArrival = DateTime.Now,
            SupplierId = arrival.SupplierId,
            Positions = arrival.ArrivalGoods.Select(x => new CreateArrivalPositionTransportModel
            {
                Uuid = x.Good.Uuid,
                PriceArrival = x.Price,
                PriceSell = 0,
                Count = x.Count,
                Nds = x.Nds,
            })
        };

        return new HttpRequestMessage(HttpMethod.Post, "create-arrival")
        {
            Content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json")
        };
        var result = await httpClient.PostAsJsonAsync("create-arrival", body);
        if (!result.IsSuccessStatusCode)
            throw new SystemException("Ошибка отправки");
    }




    public async Task<HttpRequestMessage> SendCheckSell(HttpClient httpClient, DataContext db, int docId)
    {
        var sell = db.CheckSells
            .Include(s => s.CheckGoods).ThenInclude(g => g.Good)
            .Include(s => s.CheckPayments)
            .Include(s => s.Buyer)
            .Where(s => s.Id == docId)
            .AsNoTracking().FirstOrDefault();
        var shiftbuy = db.Shifts.Where(s => s.Id == sell.ShiftId).FirstOrDefault();
        var body = new CreateCheckTransportModel
        {
            DateCreate = sell.DateCreate,
            TypeSell = sell.TypeSell,
            SumDiscont = sell.SumDiscont,
            SumCash = sell.SumCash,
            SumElectron = sell.SumElectron,
            Positions = sell.CheckGoods.Select(x => new CreateCheckPositionTransportModel
            {
                Uuid = x.Good.Uuid,
                Price = x.Cost,
                Quantity = x.Count
            })
        };

        return new HttpRequestMessage(HttpMethod.Post, "create-check")
        {
            Content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json")
        };
        var result = await httpClient.PostAsJsonAsync("create-check", body);
        if (!result.IsSuccessStatusCode)
            throw new SystemException("Ошибка отправки чека\n" + result.StatusCode);
    }

    public async Task<HttpRequestMessage> SendStocktacking(HttpClient httpClient, DataContext db, DocSynch docSynch)
    {
        var stocktaking = await db.Stocktakings
            .Include(s => s.StocktakingGroups)
            .ThenInclude(s => s.StocktakingGoods)
            .ThenInclude(g => g.Good)
            .Where(s => s.Id == docSynch.DocId)
            .AsNoTracking().FirstOrDefaultAsync();
        var body = new StocktackingTransportModel
        {
            Create = stocktaking.Create,
            CashMoney = stocktaking.CashMoney,
            Uuid = stocktaking.Uuid,
            Groups = stocktaking.StocktakingGroups.Select(x => new StocktakingGroupTransportModel
            {
                Name = x.Name,
                Goods = x.StocktakingGoods.Select(x => new StocktakingGoodTransportModel
                {
                    Uuid = x.Good.Uuid,
                    CountFact = x.CountFact ?? 0,
                })
            })
        };

        return new HttpRequestMessage(HttpMethod.Post, "create-stocktacking")
        {
            Content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json")
        };
        var result = await httpClient.PostAsJsonAsync("create-stocktacking", body);
        if (!result.IsSuccessStatusCode)
            throw new SystemException("Ошибка отправки");
    }

    public async Task<HttpRequestMessage> SendCashMoney(HttpClient httpClient, DataContext db, int docId)
    {
        var body = await db.CashMoneys.Where(x=>x.Id == docId)
            .Select(x=>new CashMoneyTransportModel
            {
                Uuid=x.Uuid,
                Create = x.Create,
                TypeOperation= x.TypeOperation,
                Note= x.Note,
                Sum= x.Sum,
            })
            .AsNoTracking().FirstOrDefaultAsync();


        return new HttpRequestMessage(HttpMethod.Post, "create-cashmoney")
        {
            Content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json")
        };
        var result = httpClient.PostAsJsonAsync("create-cashmoney", body);
        if (!result.IsCompletedSuccessfully)
            throw new Exception("Ошибка отправки CashMoney");
    }

    public async Task SynchSuppliers(HttpClient httpClient, DataContext db)
    {
        var suppliers = await httpClient.GetFromJsonAsync<IEnumerable<SupplierResponseTransportModel>>("manuals/suppliers");
        foreach (var supplier in suppliers)
        {
            var supplierDb = await db.Suppliers.Where(s => s.Id == supplier.Id).FirstOrDefaultAsync();
            if (supplierDb == null)
                db.Suppliers.Add(new Supplier { Id = supplier.Id, Name = supplier.Name, Inn = "", Kpp = "" });

        }
        await db.SaveChangesAsync();
    }
}
