using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Revaluationgood
{
    public int Id { get; set; }

    public int RevaluationId { get; set; }

    public int GoodId { get; set; }

    public decimal Count { get; set; }

    public decimal PriceOld { get; set; }

    public decimal PriceNew { get; set; }

    public virtual Good Good { get; set; } = null!;

    public virtual Revaluation Revaluation { get; set; } = null!;
}
