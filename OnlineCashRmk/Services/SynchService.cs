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
        DataContext db_;
        private readonly IDbContextFactory<DataContext> dbContextFactory;
        private readonly IHttpClientFactory httpClientFactory;
        ILogger<SynchService> logger_;
        IConfiguration configuration;
        string hostname;
        int shopId;
        private static List<DocSynch> DocSynches = new List<DocSynch>();

        public SynchService(IDbContextFactory<DataContext> dbFactory, ILogger<SynchService> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            db_ = dbFactory.CreateDbContext();
            dbContextFactory = dbFactory;
            logger_ = logger;
            this.configuration = configuration;
            this.httpClientFactory = httpClientFactory;
            hostname = configuration.GetSection("serverName").Value;
            shopId = Convert.ToInt32(configuration.GetSection("idShop").Value);
        }
        public void AppendDoc(DocSynch docSynch)
        {
            db_.DocSynches.Add(docSynch);
            db_.SaveChanges();
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
    }
}
