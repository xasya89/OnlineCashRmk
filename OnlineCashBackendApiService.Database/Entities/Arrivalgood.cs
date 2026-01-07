using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Arrivalgood
{
    public int Id { get; set; }

    public int GoodId { get; set; }

    public decimal Count { get; set; }

    public decimal Price { get; set; }

    public int ArrivalId { get; set; }

    public decimal PriceSell { get; set; }

    public string? Nds { get; set; }

    public DateOnly? ExpiresDate { get; set; }

    public virtual Arrival Arrival { get; set; } = null!;

    public virtual Good Good { get; set; } = null!;
}
