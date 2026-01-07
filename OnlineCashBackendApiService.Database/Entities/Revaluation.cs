using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Revaluation
{
    public int Id { get; set; }

    public DateOnly Create { get; set; }

    public int Status { get; set; }

    public decimal SumNew { get; set; }

    public decimal SumOld { get; set; }

    public Guid Uuid { get; set; }

    public virtual ICollection<Revaluationgood> Revaluationgoods { get; set; } = new List<Revaluationgood>();
}
