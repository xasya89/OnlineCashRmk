using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Supplier
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Inn { get; set; }

    public virtual ICollection<Arrival> Arrivals { get; set; } = new List<Arrival>();

    public virtual ICollection<Good> Goods { get; set; } = new List<Good>();
}
