using OnlineCashTransportModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashTransportModels;

public class CashMoneyTransportModel
{
    public Guid Uuid { get; set; }
    public DateTime Create { get; set; }
    public TypeCashMoneyOpertaion TypeOperation { get; set; }
    public decimal Sum { get; set; }
    public string Note { get; set; }
}
