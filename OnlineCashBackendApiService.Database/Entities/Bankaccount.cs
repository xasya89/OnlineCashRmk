using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Bankaccount
{
    public int Id { get; set; }

    public string Alias { get; set; } = null!;

    public string? BankName { get; set; }

    public string? Num { get; set; }

    public string? KorShet { get; set; }

    public string? Bik { get; set; }

    public virtual ICollection<Arrivalpayment> Arrivalpayments { get; set; } = new List<Arrivalpayment>();
}
