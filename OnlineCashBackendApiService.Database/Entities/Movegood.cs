using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Movegood
{
    public int Id { get; set; }

    public int MoveDocId { get; set; }

    public int GoodId { get; set; }

    public double Count { get; set; }

    public decimal PriceConsigner { get; set; }

    public decimal PriceConsignee { get; set; }

    public virtual Good Good { get; set; } = null!;

}
