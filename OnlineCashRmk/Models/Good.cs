using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using OnlineCashTransportModels.Shared;

namespace OnlineCashRmk.Models
{
    public class Good
    {
        public int Id { get; set; }
        [JsonPropertyName("uuid")]
        public Guid Uuid { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        public string NameLower { get; set; }
        [JsonPropertyName("article")]
        public string Article { get; set; }
        [JsonPropertyName("barCode")]
        public string BarCode { get; set; }
        [JsonPropertyName("unit")]
        public Units Unit { get; set; }
        public string UnitDescription { get => Unit.GetDisplay(); }
        public decimal PriceArrival { get; set; } = 0;
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        public decimal? PriceOld { get; set; } = null;
        public DateTime? PriceChangedDate { get; set; }
        [JsonPropertyName("specialType")]
        public SpecialTypes SpecialType { get; set; } = SpecialTypes.None;
        [JsonPropertyName("vPackage")]
        public double? VPackage { get; set; }
        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }
        public bool IsPromotion2Plus1 { get; set; }

        /// <summary>
        /// Изменение цены
        /// </summary>
        /// <param name="currentPrice"></param>
        /// <returns>True - если цена изменилась</returns>
        public bool UpdatePrice(decimal currentPrice)
        {
            if (Price != currentPrice)
            {
                PriceOld = Price;
                PriceChangedDate = DateTime.Now;
                Price = currentPrice;
                return true;
            }
            Price = currentPrice;
            return false;
        }

        public void UpdatePriceAfterRevaluation()
        {
            PriceOld = Price;
        }

        [JsonIgnore]
        public List<CheckGood> CheckGoods { get; set; }
        [JsonPropertyName("barCodes")]
        public List<BarCode> BarCodes { get; set; } = new List<BarCode>();
        [JsonIgnore]
        public List<WriteofGood> GetWriteofGoods { get; set; }
        [JsonIgnore]
        public List<ArrivalGood> ArrivalGoods { get; set; }
        [JsonIgnore]
        public List<StocktakingGood> StocktakingGoods { get; set; }
        [JsonIgnore]
        public List<NewGoodFromCash> NewGoodsFromCashe { get; set; }
        [JsonIgnore]
        public List<RevaluationGood> RevaluationGoods { get; set; }
    }
}
