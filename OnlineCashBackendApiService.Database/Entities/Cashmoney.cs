using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Cashmoney
{
    public int Id { get; set; }

    public Guid Uuid { get; set; }

    public DateTime Create { get; set; }

    public int TypeOperation { get; set; }

    public decimal Sum { get; set; }

    public string? Note { get; set; }

    public int ShopId { get; set; }

    public virtual Shop Shop { get; set; } = null!;
}
