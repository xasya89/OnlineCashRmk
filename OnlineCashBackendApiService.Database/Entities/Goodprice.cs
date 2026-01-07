using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Goodprice
{
    public int Id { get; set; }

    public int GoodId { get; set; }

    public int ShopId { get; set; }

    public decimal Price { get; set; }

    public bool? BuySuccess { get; set; }

    public virtual Good Good { get; set; } = null!;

    public virtual Shop Shop { get; set; } = null!;
}
