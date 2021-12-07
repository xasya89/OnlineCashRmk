using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Models;

namespace OnlineCashRmk.Services
{
    class SynchBuyersService
    {
        string serverName;
        public DataContext _db;
        public SynchBuyersService(DataContext db, IConfiguration configuration)
        {
            _db = db;
            serverName = configuration.GetSection("serverName").Value;
        }

        public async Task GetBuyersAsync()
        {
            var buyers = await $"{serverName}/api/onlinecash/Buyers".GetJsonAsync<List<Buyer>>();
            var buyersdb = await _db.Buyers.ToListAsync();
            foreach(var buyer in buyers)
            {
                var buyerDb = buyersdb.Where(b => b.Uuid == buyer.Uuid).FirstOrDefault();
                if (buyerDb == null)
                    _db.Buyers.Add(buyer);
                if(buyerDb!=null && buyerDb.SumBuy < buyer.SumBuy)
                {
                    buyerDb.SumBuy = buyer.SumBuy;
                    buyerDb.isChanged = false;
                }
            }
            await _db.SaveChangesAsync();
        }

        public async Task SendChangedAsync()
        {
            var buyers = await _db.Buyers.Where(b => b.isChanged == true).ToListAsync();
            await $"{serverName}/api/onlinecash/".PutJsonAsync(buyers);
            foreach (var buyer in buyers)
                buyer.isChanged = false;
            await _db.SaveChangesAsync();
        }
    }
}
