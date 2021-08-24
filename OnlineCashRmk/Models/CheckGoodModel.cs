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
        public string GoodUnit { get => Good.Unit.ToString(); }
        public decimal Sum { get => (decimal)Count * Cost; }
    }
}
