using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace OnlineCashRmk.Models
{
    public class Good
    {
        public int Id { get; set; }
        [JsonPropertyName("uuid")]
        public Guid Uuid { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("article")]
        public string Article { get; set; }
        [JsonPropertyName("barCode")]
        public string BarCode { get; set; }
        [JsonPropertyName("unit")]
        public Units Unit { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonIgnore]
        public List<CheckGood> CheckGoods { get; set; }
        [JsonPropertyName("barCodes")]
        public List<BarCode> BarCodes { get; set; } = new List<BarCode>();
    }
}
