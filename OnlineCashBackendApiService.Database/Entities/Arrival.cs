using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Arrival
{
    public int Id { get; set; }

    public string? Num { get; set; }

    public DateOnly DateArrival { get; set; }

    public int SupplierId { get; set; }

    public int ShopId { get; set; }

    public bool IsSuccess { get; set; }

    public decimal SumPayments { get; set; }

    public decimal SumArrival { get; set; }

    public decimal SumNds { get; set; }

    public decimal SumSell { get; set; }

    public int Status { get; set; }

    public virtual ICollection<Arrivalgood> Arrivalgoods { get; set; } = new List<Arrivalgood>();

    public virtual ICollection<Arrivalpayment> Arrivalpayments { get; set; } = new List<Arrivalpayment>();

    public virtual Shop Shop { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}
