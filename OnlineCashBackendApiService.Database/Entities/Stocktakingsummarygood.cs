using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Stocktakingsummarygood
{
    public int Id { get; set; }

    public int StocktakingId { get; set; }

    public int GoodId { get; set; }

    public decimal CountDb { get; set; }

    public decimal CountFact { get; set; }

    public decimal Price { get; set; }

    public virtual Good Good { get; set; } = null!;

    public virtual Stocktaking Stocktaking { get; set; } = null!;
}
