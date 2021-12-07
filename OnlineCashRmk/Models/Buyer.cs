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
        public string DiscountCardNum { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal SumBuy { get; set; }
        public bool isChanged { get; set; } = false;
    }

    public enum DiscountType
    {
        Default,
        Static
    }
}
