using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Goodbalancehistory
{
    public int Id { get; set; }

    public int ShopId { get; set; }

    public int GoodId { get; set; }

    public DateOnly CurDate { get; set; }

    public decimal CountLast { get; set; }

    public virtual Shop Shop { get; set; } = null!;
}
