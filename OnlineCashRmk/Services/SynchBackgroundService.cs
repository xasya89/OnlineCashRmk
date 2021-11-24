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
                            }
                    }
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
                    Count = aGood.Count
                });
            using (var client = new HttpClient())
                {
                    var resp = await client.PostAsJsonAsync($"{serverurl}/api/ArrivalSynch/{shopId}", model);
                    System.Diagnostics.Debug.WriteLine(resp);
                    if (resp.IsSuccessStatusCode)
                        return true;
                }
            return false;
        }
    }

    class CashBoxBuyReturnModel
    {
        public bool IsElectron { get; set; }
        public Guid Uuid { get; set; }
        public double Count { get; set; }
        public decimal Price { get; set; }
    }
}
