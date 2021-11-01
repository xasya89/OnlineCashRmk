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
        public decimal Sum { get => Math.Ceiling((decimal)Count * Cost); }
    }
}
