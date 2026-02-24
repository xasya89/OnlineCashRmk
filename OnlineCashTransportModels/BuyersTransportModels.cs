using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashTransportModels;

public class GetBuyerItemTransportModel
{
    public bool IsBlocked { get; set; }
    public Guid Uuid { get; set; }
    public string Name { get; set; }
    public string PhoneNumber {  get; set; }
    public int SpecialDiscount { get; set; }
}

public class UpdateSumDiscountBuyerTransportModel
{
    public string PhoneNumber { get; set; }
    public decimal SumDiscount { get; set; }
}
