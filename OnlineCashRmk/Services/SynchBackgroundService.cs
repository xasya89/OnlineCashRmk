using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace OnlineCashRmk.Services
{
    class SynchBackgroundService:ISynchBackgroundService
    {
        public SynchBackgroundService(IServiceProvider provider)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    DataContext db = provider.GetRequiredService<DataContext>();
                    IConfiguration config = provider.GetRequiredService<IConfiguration>();
                    var serverurl = config.GetSection("serverName").Value;
                    int shopId = Convert.ToInt32(config.GetSection("idShop").Value);
                    int cronSynch = Convert.ToInt32(config.GetSection("Crons").GetSection("Synch").Value);
                    IEnumerable<DocSynch> docs = db.DocSynches.Where(d => d.SynchStatus == false).OrderBy(d=>d.Create);
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
                                    foreach(var checkgood in check.CheckGoods)
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
                                        if(resp.IsSuccessStatusCode)
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
                            }
                    }
                    catch (HttpRequestException) { }
                    catch (Exception) { };
                    await Task.Delay(TimeSpan.FromMinutes(cronSynch));
                }
            });

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
