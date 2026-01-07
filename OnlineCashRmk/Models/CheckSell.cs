using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineCashTransportModels.Shared;

namespace OnlineCashRmk.Models
{
    public class CheckSell
    {
        public int Id { get; set; }
        public int? BuyerId { get; set; }
        [ForeignKey("BuyerId")]
        public Buyer Buyer { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public TypeSell TypeSell { get; set; } = TypeSell.Sell;
        public bool IsReturn
        {
            get => TypeSell == TypeSell.Return; 
        }
        public bool IsElectron { get; set; }
        public decimal Sum { get; set; }
        public decimal SumDiscont { get; set; } = 0;
        public decimal SumElectron { get; set; }
        public decimal SumCash { get; set; }
        public decimal SumAll { get; set; }
        [JsonPropertyName("Goods")]
        public ICollection<CheckGood> CheckGoods { get; set; }
        public ICollection<CheckPayment> CheckPayments { get; set; }
        public int ShiftId { get; set; }
        [JsonIgnore]
        public Shift Shift { get; set; }
    }
}
