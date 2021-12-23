using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Models
{
    public class StocktakingGroup
    {
        public int Id { get; set; }
        public int StocktakingId { get; set; }
        public Stocktaking Stocktaking { get; set; }
        public string Name { get; set; }
        public List<StocktakingGood> StocktakingGoods { get; set; } = new List<StocktakingGood>();
    }
}
