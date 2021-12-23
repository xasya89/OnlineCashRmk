using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Models
{
    public class Stocktaking
    {
        public int Id { get; set; }
        public DateTime Create { get; set; } = DateTime.Now;
        public bool isSynch { get; set; }
        public List<StocktakingGroup> StocktakingGroups { get; set; } = new List<StocktakingGroup>();
    }
}
