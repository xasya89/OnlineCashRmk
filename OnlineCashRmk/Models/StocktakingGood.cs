using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Globalization;

namespace OnlineCashRmk.Models
{
    public class StocktakingGood
    {
        public int Id { get; set; }
        public int StocktakingGroupId { get; set; }
        public StocktakingGroup StocktakingGroup { get; set; }
        public int GoodId { get; set; }
        public Good Good { get; set; }
        public Guid Uuid { get => Good.Uuid; }
        public string GoodName { get => Good?.Name; }
        public string UnitStr { get => Good?.Unit.DisplayName(); }
        public string CountFactStr { 
            get => CountFact?.ToString();
            set
            {
                decimal countFact = -1;
                decimal.TryParse(value?.Replace(",","."), NumberStyles.Any, CultureInfo.InvariantCulture, out countFact);
                CountFact = countFact == -1 ? null : countFact;
            }
        }
        public decimal? CountFact { get; set; }
        [JsonIgnore]
        public decimal? Price { get => Good?.Price; }
        [JsonIgnore]
        public decimal? Sum { get => Price * CountFact; }
        [JsonIgnore]
        public decimal? CountDocMove { get; set; }
    }
}
