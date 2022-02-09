using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Models;
using OnlineCashRmk.DataModels;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Flurl.Http;

namespace OnlineCashRmk.Services
{
    class SynchBackgroundService : ISynchBackgroundService
    {
        DataContext db;
        string serverurl;
        int shopId;
        int cronSynch;
        public SynchBackgroundService(IServiceProvider provider)
        {
            db = provider.GetRequiredService<DataContext>();
            IConfiguration config = provider.GetRequiredService<IConfiguration>();
            SynchBuyersService buyersService = provider.GetRequiredService<SynchBuyersService>();
            serverurl = config.GetSection("serverName").Value;
            shopId = Convert.ToInt32(config.GetSection("idShop").Value);
            cronSynch = Convert.ToInt32(config.GetSection("Crons").GetSection("Synch").Value);
            Task.Run(async () =>
            {
                while (true)
                {
                    IEnumerable<DocSynch> docs = db.DocSynches.Where(d => d.SynchStatus == false).OrderBy(d => d.Create);
                    try
                    {
                        foreach (var doc in docs)
                            switch (doc.TypeDoc)
                            {
                                case TypeDocs.OpenShift:
                                    var shift = await db.Shifts.Where(s => s.Id == doc.DocId).FirstOrDefaultAsync();
                                    using (var client = new HttpClient())
                                    {
                                        var resp = await client.PostAsync($"{serverurl}/api/Cashbox/OpenShift/{shopId}/{shift.Uuid}/{shift.Start.ToString()}", null);
                                        if (resp.IsSuccessStatusCode)
                                        {
                                            doc.SynchStatus = true;
                                            doc.Synch = DateTime.Now;
                                            await db.SaveChangesAsync();
                                        }
                                        else
                                            throw new Exception("Ошибка отправки на сервер");
                                    }
                                    break;
                                case TypeDocs.Buy:
                                    /*
                                    var check = await db.CheckSells
                                    .Include(c => c.CheckGoods).ThenInclude(cg => cg.Good)
                                    .Where(c => c.Id == doc.DocId).FirstOrDefaultAsync();
                                    var shiftbuy = await db.Shifts.Where(s => s.Id == check.ShiftId).FirstOrDefaultAsync();
                                    List<CashBoxBuyReturnModel> bues = new List<CashBoxBuyReturnModel>();
                                    foreach (var checkgood in check.CheckGoods)
                                    {
                                        bool isElectron = check.IsElectron;
                                        Guid uuid = checkgood.Good.Uuid;
                                        double count = checkgood.Count;
                                        decimal price = checkgood.Cost;
                                        var buy = bues.Where(b => b.Uuid == uuid).FirstOrDefault();
                                        if (buy == null)
                                            bues.Add(new CashBoxBuyReturnModel
                                            {
                                                IsElectron = isElectron,
                                                Uuid = uuid,
                                                Count = count,
                                                Price = price
                                            });
                                        else
                                            buy.Count += count;
                                    };
                                    using (var client = new HttpClient())
                                    {
                                        var resp = await client.PostAsJsonAsync($"{serverurl}/api/CashBox/buy/{shiftbuy.Uuid}", bues);
                                        if (resp.IsSuccessStatusCode)
                                        {
                                            doc.SynchStatus = true;
                                            doc.Synch = DateTime.Now;
                                            await db.SaveChangesAsync();
                                        }
                                    }
                                    */
                                    await SendCheckSell(doc.DocId);
                                    doc.SynchStatus = true;
                                    doc.Synch = DateTime.Now;
                                    await db.SaveChangesAsync();
                                    break;
                                case TypeDocs.CloseShift:
                                    using (var client = new HttpClient())
                                    {
                                        var shiftclose = await db.Shifts.Where(s => s.Id == doc.DocId).FirstOrDefaultAsync();
                                        var resp = await client.PostAsync($"{serverurl}/api/Cashbox/CloseShift/{shiftclose.Uuid}/{shiftclose.Stop.ToString()}", null);
                                        if (resp.IsSuccessStatusCode)
                                        {
                                            doc.SynchStatus = true;
                                            doc.Synch = DateTime.Now;
                                            await db.SaveChangesAsync();
                                        }
                                        else
                                            throw new Exception("Ошибка отправки на сервер");
                                    }
                                    break;
                                case TypeDocs.WriteOf:
                                    var writeof = await db.Writeofs.Include(w => w.WriteofGoods).ThenInclude(wg => wg.Good).Where(w => w.Id == doc.DocId).FirstOrDefaultAsync();
                                    List<WriteofGoodSynchDataModel> goods = new List<WriteofGoodSynchDataModel>();
                                    foreach (var wg in writeof.WriteofGoods)
                                        goods.Add(new WriteofGoodSynchDataModel { Uuid = wg.Good.Uuid, Count = wg.Count, Price = wg.Price });
                                    var writeofsynch = new WriteofSynchDataModel { DateCreate = writeof.DateCreate, Note = writeof.Note, Goods = goods };
                                    using (var client = new HttpClient())
                                    {
                                        var resp = await client.PostAsJsonAsync($"{serverurl}/api/WriteofSynch/{shopId}", writeofsynch);
                                        if (resp.IsSuccessStatusCode)
                                        {
                                            doc.SynchStatus = true;
                                            doc.Synch = DateTime.Now;
                                            await db.SaveChangesAsync();
                                        }
                                    }
                                    break;
                                case TypeDocs.Arrival:
                                    if (await SendArrival(doc.DocId))
                                    {
                                        doc.SynchStatus = true;
                                        doc.Synch = DateTime.Now;
                                        await db.SaveChangesAsync();
                                    }
                                    break;
                                case TypeDocs.StockTaking:
                                    await SendStocktaking(doc.DocId);
                                    doc.SynchStatus = true;
                                    doc.Synch = DateTime.Now;
                                    await db.SaveChangesAsync();
                                    break;
                                case TypeDocs.CashMoney:
                                    await SendCashMoney(doc.DocId);
                                    doc.SynchStatus = true;
                                    doc.Synch = DateTime.Now;
                                    await db.SaveChangesAsync();
                                    break;
                                case TypeDocs.NewGoodFromCash:
                                    await SendNewGood(doc.DocId);
                                    doc.SynchStatus = true;
                                    doc.Synch = DateTime.Now;
                                    await db.SaveChangesAsync();
                                    break;
                            }

                        await GetBuyersAsync();
                        if (db.Buyers.Where(b => b.isChanged == true).Count() > 0)
                            await SendChangedAsync();
                    }
                    catch (FlurlHttpException) { }
                    catch (HttpRequestException) { }
                    catch (Exception) { }
                    await Task.Delay(TimeSpan.FromMinutes(cronSynch));
                }
            });

        }

        private async Task<Boolean> SendArrival(int docId)
        {
            var arrival = await db.Arrivals.Include(a => a.ArrivalGoods).ThenInclude(a => a.Good).Where(a => a.Id == docId).FirstOrDefaultAsync();
            if (arrival == null)
                return false;
            var model = new ArrivalSynchDataModel { Num = arrival.Num, DateArrival = arrival.DateArrival, SupplierId = arrival.SupplierId, Uuid = arrival.Uuid };
            foreach (var aGood in arrival.ArrivalGoods)
                model.ArrivalGoods.Add(new ArrivalGoodSynchDataModel
                {
                    GoodUuid = aGood.Good.Uuid,
                    Price = aGood.Price,
                    Count = aGood.Count,
                    Nds=aGood.Nds,
                    ExpiresDate=aGood.ExpiresDate
                });
            using (var client = new HttpClient())
            {
                var resp = await client.PostAsJsonAsync($"{serverurl}/api/onlinecash/ArrivalSynch/{shopId}", model);
                if (resp.IsSuccessStatusCode)
                    return true;
            }
            return false;
        }


        public async Task GetBuyersAsync()
        {
            var buyers = await $"{serverurl}/api/onlinecash/Buyers".GetJsonAsync<List<Buyer>>();
            var buyersdb = await db.Buyers.ToListAsync();
            foreach (var buyer in buyers)
            {
                var buyerDb = buyersdb.Where(b => b.Uuid == buyer.Uuid).FirstOrDefault();
                if (buyerDb == null)
                    db.Buyers.Add(buyer);
                if (buyerDb != null && buyerDb.SumBuy < buyer.SumBuy)
                {
                    buyerDb.SumBuy = buyer.SumBuy;
                    buyerDb.isChanged = false;
                }
            }
            await db.SaveChangesAsync();
        }

        public async Task SendCheckSell(int docId)
        {
            var sell = db.CheckSells.Include(s => s.CheckGoods).ThenInclude(g=>g.Good).Include(s => s.CheckPayments).Where(s => s.Id == docId).FirstOrDefault();
            var shiftbuy = db.Shifts.Where(s => s.Id == sell.ShiftId).FirstOrDefault();
            var sellPost = new CashBoxCheckSellModel
            {
                IsReturn=sell.IsReturn,
                Create = sell.DateCreate,
                SumCash = sell.CheckPayments.Where(p => p.TypePayment == TypePayment.Cash).Sum(p => p.Sum),
                SumElectron = sell.CheckPayments.Where(p => p.TypePayment == TypePayment.Electron).Sum(p => p.Sum),
                SumDiscount = sell.SumDiscont
            };
            foreach (var g in sell.CheckGoods)
                sellPost.Goods.Add(new CashBoxCheckSellGood
                {
                    Uuid = g.Good.Uuid,
                    Count = (decimal)g.Count,
                    Discount = 0,
                    Price = g.Cost
                });
            await $"{serverurl}/api/CashBox/sell/{shiftbuy.Uuid}".PostJsonAsync(sellPost);
        }

        public async Task SendChangedAsync()
        {
            var buyers = await db.Buyers.Where(b => b.isChanged == true).ToListAsync();
            await $"{serverurl}/api/onlinecash/Buyers".PutJsonAsync(buyers);
            foreach (var buyer in buyers)
                buyer.isChanged = false;
            await db.SaveChangesAsync();
        }

        public async Task SendStocktaking(int docId)
        {
            var stocktaking = await db.Stocktakings.Include(s => s.StocktakingGroups).ThenInclude(s => s.StocktakingGoods).ThenInclude(g => g.Good).Where(s => s.Id == docId).FirstOrDefaultAsync();
            var stocktakingSend = new StocktakingSendDataModel { Create = stocktaking.Create };
            foreach (var group in stocktaking.StocktakingGroups)
            {
                var groupSend = new StocktakingGroupSendDataModel { Name = group.Name };
                foreach (var good in group.StocktakingGoods)
                {
                    double countfact = 0;
                    if (good.CountFact != null)
                        countfact = (double)good.CountFact;
                    groupSend.Goods.Add(new StocktakingGoodSendDataModel { Uuid = good.Uuid, CountFact = countfact });
                };
                stocktakingSend.Groups.Add(groupSend);
            }
            try
            {
                var result = await $"{serverurl}/api/onlinecash/Stocktaking/{shopId}".PostJsonAsync(stocktakingSend);
            }
            catch (FlurlHttpException ex)
            {
            }
        }

        public async Task SendCashMoney(int docId)
        {
            var cashMoney = db.CashMoneys.Where(c => c.Id == docId).FirstOrDefault();
            if (cashMoney == null)
                throw new Exception("Не найден документ cashMoney");
            await $"{serverurl}/api/onlinecash/CashMoneys/{shopId}".PostJsonAsync(cashMoney);
        }

        public async Task SendNewGood(int docId)
        {
            var good = db.NewGoodsFromCash.Include(n => n.Good)
                .ThenInclude(g=>g.BarCodes)
                .Where(n => n.Id == docId).FirstOrDefault()?.Good;
            var goodSynch = new GoodSynchDataModel
            {
                Uuid = good.Uuid,
                Name = good.Name,
                Unit = good.Unit,
                SpecialType = good.SpecialType,
                VPackage = good.VPackage,
                Price = good.Price,
                IsDeleted = false,
                Barcodes=good.BarCodes.Select(b=>b.Code).ToList()
            };
            await $"{serverurl}/api/onlinecash/NewGoodFromCashSynch/{shopId}".PostJsonAsync(goodSynch);
        }
    }

    class CashBoxCheckSellGood
    {
        public Guid Uuid { get; set; }
        public decimal Count { get; set; }
        public decimal Discount { get; set; }
        public decimal Price { get; set; }
    }

    class CashBoxCheckSellModel
    {
        public DateTime Create { get; set; }
        public bool IsReturn { get; set; } = false;
        public decimal SumCash { get; set; }
        public decimal SumElectron { get; set; }
        public decimal SumDiscount { get; set; }
        public List<CashBoxCheckSellGood> Goods { get; set; } = new List<CashBoxCheckSellGood>();
    }
}
