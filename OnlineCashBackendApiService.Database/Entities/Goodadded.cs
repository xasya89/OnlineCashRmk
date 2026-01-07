using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Goodadded
{
    public int Id { get; set; }

    public int GoodId { get; set; }

    public virtual Good Good { get; set; } = null!;
}
