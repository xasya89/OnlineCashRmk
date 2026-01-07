using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Writeofgood
{
    public int Id { get; set; }

    public int WriteofId { get; set; }

    public int GoodId { get; set; }

    public decimal Count { get; set; }

    public decimal Price { get; set; }

    public virtual Good Good { get; set; } = null!;

    public virtual Writeof Writeof { get; set; } = null!;
}
