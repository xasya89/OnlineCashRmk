using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlineCashRmk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Json;
using OnlineCashTransportModels;

namespace OnlineCashRmk.Services
{
    public class SynchService : ISynchService
    {
        private readonly IDbContextFactory<DataContext> dbContextFactory;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly HttpClient httpClient;
        ILogger<SynchService> logger_;
        IConfiguration configuration;
        string hostname;
        int shopId;
        private static List<DocSynch> DocSynches = new List<DocSynch>();

        public SynchService(IDbContextFactory<DataContext> dbFactory, ILogger<SynchService> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            dbContextFactory = dbFactory;
            httpClient = httpClientFactory.CreateClient(Program.HttpClientName);
            logger_ = logger;
            this.configuration = configuration;
            this.httpClientFactory = httpClientFactory;
            hostname = configuration.GetSection("serverName").Value;
            shopId = Convert.ToInt32(configuration.GetSection("idShop").Value);
        }
        public void AppendDoc(DocSynch docSynch)
        {
            using var db = dbContextFactory.CreateDbContext();
            db.DocSynches.Add(docSynch);
            db.SaveChanges();
            DocSynches.Add(docSynch);
        }

        public async Task<List<Supplier>> SynchSuppliersAsync()
        {
            var client = httpClientFactory.CreateClient(Program.HttpClientName);
            var suppliers = await client.GetFromJsonAsync<IEnumerable<SupplierResponseTransportModel>>("/manuals/suppliers");
            using var db = dbContextFactory.CreateDbContext();
            foreach(var supplier in suppliers)
            {
                var supplierDb = db.Suppliers.Where(s => s.Id == supplier.Id).FirstOrDefault();
                if (supplierDb == null)
                    db.Suppliers.Add(new Supplier { Id = supplier.Id, Name = supplier.Name, Inn = "", Kpp = "" });

            }
            db.SaveChanges();
            return await db.Suppliers.OrderBy(x=>x.Name).AsNoTracking().ToListAsync();
        }

        public async Task SynchGoods()
        {
            using var db = dbContextFactory.CreateDbContext();
            var goods = await httpClient.GetFromJsonAsync<IEnumerable<GoodsResponseTransportModel>>($"manuals/goods");
            foreach (var good in goods)
            {
                var goodDb = db.Goods.Include(g => g.BarCodes).Where(g => g.Uuid == good.Uuid).FirstOrDefault();
                if (goodDb == null)
                {
                    var newgood = new Good
                    {
                        Uuid = good.Uuid,
                        Name = good.Name,
                        NameLower = good.Name.Trim().ToLower(),
                        Article = good.Name,
                        Unit = good.Unit,
                        Price = good.Price,
                        SpecialType = good.SpecialType,
                        VPackage = good.VPackage,
                        IsDeleted = good.IsDeleted
                    };
                    db.Goods.Add(newgood);
                    //добавление штрих кодов
                    foreach (string barcode in good.Barcodes)
                        db.BarCodes.Add(new BarCode
                        {
                            Good = newgood,
                            Code = barcode
                        });
                }
                else
                {
                    goodDb.Name = good.Name;
                    goodDb.NameLower = good.Name.Trim().ToLower();
                    goodDb.Unit = good.Unit;
                    goodDb.Price = good.Price;
                    goodDb.SpecialType = good.SpecialType;
                    goodDb.VPackage = good.VPackage;
                    goodDb.IsDeleted = good.IsDeleted;
                    //добавление новых или измененных штрих кодов
                    foreach (string barcode in good.Barcodes)
                        if (goodDb.BarCodes.Count(b => b.Code == barcode) == 0)
                            db.BarCodes.Add(new BarCode { Good = goodDb, Code = barcode });
                    //Удаление не зарегестрированных на сервере штрихкодов
                    foreach (var barcodeDb in goodDb.BarCodes)
                        if (good.Barcodes.Count(b => b == barcodeDb.Code) == 0)
                            db.BarCodes.Remove(barcodeDb);
                }
            }
            ;
            await db.SaveChangesAsync();
        }
    }
}
