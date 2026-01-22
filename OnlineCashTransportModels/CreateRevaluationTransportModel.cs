using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashTransportModels;

public class CreateRevaluationTransportModel
{
    public Guid Uuid {  get; set; }
    public DateTime Create {  get; set; }
    public IEnumerable<CreateRevaluationItemTransportModel> Items { get; set; }
}
public class CreateRevaluationItemTransportModel
{
    public Guid GoodUuid { get; set; }
    public decimal Quantity { get; set; }
    public decimal PriceOld { get; set; }
    public decimal PriceNew { get; set; }
}
