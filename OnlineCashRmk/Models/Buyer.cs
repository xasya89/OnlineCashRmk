using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnlineCashRmk.Models
{
    public class Buyer
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int SpecialPercent { get; set; }
        public List<CheckSell> CheckSells { get; set; }
    }
}
