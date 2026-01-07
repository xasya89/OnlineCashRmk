using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Newgoodfromcash
{
    public int Id { get; set; }

    public int GoodId { get; set; }

    public bool Processed { get; set; }

    public bool IsNewGood { get; set; }

    public virtual Good Good { get; set; } = null!;
}
