using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Models
{
    public class Buyer
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; } = null;
        public string Phone { get; set; }
        public string DiscountCardNum { get; set; }
        public DiscountType DiscountType { get; set; }
        public int? DiscountPercant { get; set; }
        public decimal? DiscountSum { get; set; }
        public decimal SumBuy { get; set; }
        public bool isChanged { get; set; } = false;

        public List<CheckSell> CheckSells { get; set; }
    }

    public enum DiscountType
    {
        Default,
        Static
    }
}
