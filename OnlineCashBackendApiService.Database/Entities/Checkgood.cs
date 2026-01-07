using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Checkgood
{
    public int Id { get; set; }

    public decimal Count { get; set; }

    public decimal Price { get; set; }

    public int GoodId { get; set; }

    public int CheckSellId { get; set; }

    public virtual Checksell CheckSell { get; set; } = null!;

    public virtual Good Good { get; set; } = null!;
}
