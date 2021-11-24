using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCashRmk.Models;

namespace OnlineCashRmk.DataModels
{
    public class ArrivalPositionDataModel
    {
        public int GoodId { get; set; }
        public string GoodName { get; set; }
        public Units Unit { get; set; }
        public string UnitStr
        {
            get => Unit.GetDisplay();
        }
        public decimal PriceArrival { get; set; }
        public decimal PriceSell { get; set; }
        public decimal? PricePercent
        {
            get => PriceArrival==0? null : Math.Round(PriceSell / PriceArrival * 100,2);
        }
        public decimal Count { get; set; }
        public decimal SumArrival
        {
            get => PriceArrival * (decimal)Count;
        }
        public decimal SumSell
        {
            get => PriceSell * (decimal)Count;
        }
    }
}
