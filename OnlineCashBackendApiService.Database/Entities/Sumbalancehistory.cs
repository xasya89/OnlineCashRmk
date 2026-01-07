using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Sumbalancehistory
{
    public int Id { get; set; }

    public int ShopId { get; set; }

    public DateOnly DateHistory { get; set; }

    public decimal StartDaySum { get; set; }

    public decimal StocktakingSum { get; set; }

    public decimal ArrivalSum { get; set; }

    public decimal WriteofSum { get; set; }

    public decimal MoveSum { get; set; }

    public decimal CashSalesSum { get; set; }

    public decimal CashReturnSum { get; set; }

    public decimal CashIncomeSum { get; set; }

    public decimal CashOutcomeSum { get; set; }

    public decimal EndDaySum { get; set; }

    public virtual Shop Shop { get; set; } = null!;
}
