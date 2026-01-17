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
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Hosting;
using System.Threading;
using OnlineCashTransportModels;
using Microsoft.Extensions.Logging;

namespace OnlineCashRmk.Services
{
    class SynchBackgroundService(IDocumentSenderService documentSenderService, ILogger<SynchBackgroundService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            PeriodicTimer periodicTimer = new PeriodicTimer(TimeSpan.FromMinutes(5));
            while (await periodicTimer.WaitForNextTickAsync() & !stoppingToken.IsCancellationRequested)
                try
                {
                    await documentSenderService.SendDocuments();
                }
                catch(SystemException ex)
                {
                    logger.LogError("Ошибка обмена с сервером");
                }
                catch (Exception ex) {
                    logger.LogError("Ошибка обмена с сервером");
                }
        }
    

        /*
        public async Task DoWork()
        {
            var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<IDbContextFactory<DataContext>>().CreateDbContext();
            var httpClient = httpClientFactory.CreateClient(Program.HttpClientName);
            IEnumerable<DocSynch> docs = await db.DocSynches.Where(d => d.SynchStatus == false).OrderBy(d => d.Create).ToListAsync();
            try
            {
                foreach (var doc in docs)
                {
                    if (doc.Uuid.ToString() == "00000000-0000-0000-0000-000000000000")
                        doc.Uuid = Guid.NewGuid();
                    switch (doc.TypeDoc)
                    {
                        case TypeDocs.OpenShift:
                            await SendOpenShift(httpClient, db, doc.DocId);
                            break;
                        case TypeDocs.Buy:
                            await SendCheckSell(httpClient, db, doc.DocId);
                            break;
                        case TypeDocs.CloseShift:
                            await SendCloseShift(httpClient, db, doc.DocId);
                            break;
                        case TypeDocs.WriteOf:
                            await SendWriteOf(httpClient, db, doc.DocId);

                            break;
                        case TypeDocs.Arrival:
                            await SendArrival(httpClient, db, doc);
                            break;
                        case TypeDocs.StockTaking:
                            break;
                        case TypeDocs.StopStocktacking:
                            await SendStocktacking(httpClient, db, doc);
                            break;
                        case TypeDocs.CashMoney:
                            //await SendCashMoney(doc);
                            break;
                        case TypeDocs.Revaluation:
                            //await SendRevaluation(doc);
                            break;
                    }
                    doc.SynchStatus = true;
                    doc.Synch = DateTime.Now;
                    await db.SaveChangesAsync();
                }

                await GetSuppliers(httpClient, db);
            }
            catch (FlurlHttpException) { }
            catch (HttpRequestException) { }
            catch (SystemException ex)
            {
                _logger.LogError(nameof(SynchBackgroundService) + " \nОшибка: " + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(nameof(SynchBackgroundService) + " \nОшибка: " + ex.Message);
            }
        }
        */
        private async Task SendOpenShift(HttpClient httpClient, DataContext db, int shiftId)
        {
            var shift = await db.Shifts.Where(s => s.Id ==shiftId)
                .AsNoTracking().FirstOrDefaultAsync();
            var result = await httpClient.PostAsJsonAsync("open-shift", new OpenShiftTransportModel(shift.Start, shift.Uuid));
            
            if (!result.IsSuccessStatusCode)
                throw new SystemException("Ошибка отправки открытия смены");
        }

        private async Task SendCloseShift(HttpClient httpClient, DataContext db, int shiftId)
        {
            var shiftclose = await db.Shifts.Where(s => s.Id == shiftId)
                .AsNoTracking().FirstOrDefaultAsync();
            var result = await httpClient.PostAsJsonAsync("close-shift", new CloseShiftTransportModel(shiftclose.Stop.Value, shiftclose.Uuid));
            if (!result.IsSuccessStatusCode)
                throw new SystemException("Ошибка отправки закрытия смены");
        }

        private async Task SendWriteOf(HttpClient httpClient, DataContext db, int writeOfId)
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
            var result = await httpClient.PostAsJsonAsync("create-writeof", body);
            if (!result.IsSuccessStatusCode)
                throw new SystemException("Ошибка отправки");
        }

        private async Task SendArrival(HttpClient httpClient, DataContext db, DocSynch docSynch)
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
                    PriceSell=0,
                    Count = x.Count,
                    Nds = x.Nds,
                })
            };
            var result = await httpClient.PostAsJsonAsync("create-arrival", body);
            if (!result.IsSuccessStatusCode)
                throw new SystemException("Ошибка отправки");
        }




        public async Task SendCheckSell(HttpClient httpClient, DataContext db, int docId)
        {
            var sell = db.CheckSells
                .Include(s => s.CheckGoods).ThenInclude(g=>g.Good)
                .Include(s => s.CheckPayments)
                .Include(s=>s.Buyer)
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
            var result = await httpClient.PostAsJsonAsync("create-check", body);
            if (!result.IsSuccessStatusCode)
                throw new SystemException("Ошибка отправки чека\n"+result.StatusCode);
        }

        public async Task SendStocktacking(HttpClient httpClient, DataContext db, DocSynch docSynch)
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
            var result = await httpClient.PostAsJsonAsync("create-stocktacking", body);
            if (!result.IsSuccessStatusCode)
                throw new SystemException("Ошибка отправки");
        }

        public async Task GetSuppliers(HttpClient httpClient, DataContext db)
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
}
