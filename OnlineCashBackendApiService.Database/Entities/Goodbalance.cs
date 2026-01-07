using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Goodbalance
{
    public int Id { get; set; }

    public int ShopId { get; set; }

    public int GoodId { get; set; }

    public double Count { get; set; }

    public virtual Shop Shop { get; set; } = null!;
}
