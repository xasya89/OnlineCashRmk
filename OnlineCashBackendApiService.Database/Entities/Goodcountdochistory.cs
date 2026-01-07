using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Goodcountdochistory
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int GoodId { get; set; }

    public int DocId { get; set; }

    public int TypeDoc { get; set; }

    public decimal Count { get; set; }

    public virtual Good Good { get; set; } = null!;
}
