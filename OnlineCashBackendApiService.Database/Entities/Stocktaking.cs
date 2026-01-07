using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Stocktaking
{
    public int Id { get; set; }

    public int Num { get; set; }

    public DateOnly? Create { get; set; }

    public int ShopId { get; set; }

    public bool IsSuccess { get; set; }

    public int Status { get; set; }

    public double CountDb { get; set; }

    public double CountFact { get; set; }

    public decimal SumDb { get; set; }

    public decimal SumFact { get; set; }

    public DateTime Start { get; set; }

    public decimal CashMoneyDb { get; set; }

    public decimal CashMoneyFact { get; set; }

    public Guid Uuid { get; set; }

    public virtual ICollection<Reportsafterstocktaking> Reportsafterstocktakings { get; set; } = new List<Reportsafterstocktaking>();

    public virtual Shop Shop { get; set; } = null!;

    public virtual ICollection<Stocktakinggroup> Stocktakinggroups { get; set; } = new List<Stocktakinggroup>();

    public virtual ICollection<Stocktakingsummarygood> Stocktakingsummarygoods { get; set; } = new List<Stocktakingsummarygood>();
}
