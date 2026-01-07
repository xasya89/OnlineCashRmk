using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class PriceControll
{
    public int Id { get; set; }

    public int GoodId { get; set; }

    public decimal PrevPrice { get; set; }

    public virtual Good Good { get; set; } = null!;
}
