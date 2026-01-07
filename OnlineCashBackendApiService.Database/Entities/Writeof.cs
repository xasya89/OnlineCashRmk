using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Writeof
{
    public int Id { get; set; }

    public DateOnly DateWriteof { get; set; }

    public int ShopId { get; set; }

    public string? Note { get; set; }

    public decimal SumAll { get; set; }

    public bool IsSuccess { get; set; }

    public int Status { get; set; }

    public Guid? Uuid { get; set; }

    public virtual Shop Shop { get; set; } = null!;

    public virtual ICollection<Writeofgood> Writeofgoods { get; set; } = new List<Writeofgood>();
}
