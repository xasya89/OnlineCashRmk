using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class PriceControllPosition
{
    public int Id { get; set; }

    public int GoodId { get; set; }

    public decimal PriceOld { get; set; }

    public decimal PriceNew { get; set; }

    public DateOnly DateChecnge { get; set; }

    public bool IsChange { get; set; }

    public virtual Good Good { get; set; } = null!;
}
