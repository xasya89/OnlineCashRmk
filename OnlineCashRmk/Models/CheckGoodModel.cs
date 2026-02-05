using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Models
{
    public class CheckGoodModel:CheckGood
    {
        public string GoodName { get => Good?.Name; }
        public string GoodUnit { get => Good.Unit.GetDisplay(); }
        public decimal PromotionQuantity { get; set; }

        private decimal discount;
        public decimal Discount
        {
            get => discount;
            set
            {
                discount = value;
                Cost = Cost - Cost * discount / 100;
            }
        }
        public decimal Sum { get => Math.Round( (Count - PromotionQuantity) * Cost,2, MidpointRounding.AwayFromZero); }
    }
}
