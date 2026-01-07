using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Reportsafterstocktaking
{
    public int Id { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly StopDate { get; set; }

    public decimal StocktakingPrependSum { get; set; }

    public decimal CashStartSum { get; set; }

    public decimal IncomeSum { get; set; }

    public decimal ElectronSum { get; set; }

    public decimal ArrivalSum { get; set; }

    public decimal WriteOfSum { get; set; }

    public decimal CashEndSum { get; set; }

    public decimal StocktakingFactSum { get; set; }

    public int StocktakingId { get; set; }

    public decimal RevaluationArrival { get; set; }

    public decimal RevaluationWriteOf { get; set; }

    public virtual Stocktaking Stocktaking { get; set; } = null!;
}
