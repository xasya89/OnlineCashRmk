using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Goodgroup
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Good> Goods { get; set; } = new List<Good>();
}
