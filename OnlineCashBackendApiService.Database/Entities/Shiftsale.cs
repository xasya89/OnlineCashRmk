using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Shiftsale
{
    public int Id { get; set; }

    public int ShiftId { get; set; }

    public int GoodId { get; set; }

    public double Count { get; set; }

    public decimal Price { get; set; }

    public decimal CountReturn { get; set; }

    public decimal PriceReturn { get; set; }

    public virtual Good Good { get; set; } = null!;

    public virtual Shift Shift { get; set; } = null!;
}
