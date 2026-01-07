using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Cashier
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Inn { get; set; }

    public string? PinCode { get; set; }

    public bool IsBlocked { get; set; }

    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
