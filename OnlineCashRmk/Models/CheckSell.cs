using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace OnlineCashRmk.Models
{
    public class CheckSell
    {
        public int Id { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public bool IsElectron { get; set; }
        public decimal Sum { get; set; }
        public decimal SumDiscont { get; set; } = 0;
        public decimal SumAll { get; set; }
        [JsonPropertyName("Goods")]
        public List<CheckGood> CheckGoods { get; set; } = new List<CheckGood>();
        public List<CheckPayment> CheckPayments { get; set; } = new List<CheckPayment>();
        public int ShiftId { get; set; }
        [JsonIgnore]
        public Shift Shift { get; set; }
    }
}
