using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Goodcountbalance
{
    public int Id { get; set; }

    public DateTime Period { get; set; }

    public int GoodId { get; set; }

    public decimal Count { get; set; }

    public virtual Good Good { get; set; } = null!;
}
