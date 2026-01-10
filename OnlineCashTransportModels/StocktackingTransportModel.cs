using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashTransportModels;

public class StocktackingTransportModel
{
    public DateTime Create { get; set; }
    public Guid Uuid { get; set; }
    public decimal CashMoney { get; set; }
    public IEnumerable<StocktakingGroupTransportModel> Groups { get; set; }
}

public class StocktakingGroupTransportModel
{
    public string Name { get; set; }
    public IEnumerable<StocktakingGoodTransportModel> Goods { get; set; }
}
public class StocktakingGoodTransportModel
{
    public Guid Uuid { get; set; }
    public decimal CountFact { get; set; }
}