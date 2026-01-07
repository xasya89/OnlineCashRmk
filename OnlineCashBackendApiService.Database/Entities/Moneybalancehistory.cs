using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Moneybalancehistory
{
    public int Id { get; set; }

    public int ShopId { get; set; }

    public DateTime DateBalance { get; set; }

    public decimal SumSale { get; set; }

    public decimal SumReturn { get; set; }

    public decimal SumIncome { get; set; }

    public decimal SumOutcome { get; set; }

    public decimal SumOther { get; set; }

    public decimal SumEnd { get; set; }

    public decimal SumStart { get; set; }

    public virtual Shop Shop { get; set; } = null!;
}
