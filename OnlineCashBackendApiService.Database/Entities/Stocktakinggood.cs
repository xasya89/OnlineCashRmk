using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Stocktakinggood
{
    public int Id { get; set; }

    public int GoodId { get; set; }

    public double Count { get; set; }

    public double CountFact { get; set; }

    public int StockTakingGroupId { get; set; }

    public double CountDb { get; set; }

    public decimal Price { get; set; }

    public virtual Good Good { get; set; } = null!;

    public virtual Stocktakinggroup StockTakingGroup { get; set; } = null!;
}
