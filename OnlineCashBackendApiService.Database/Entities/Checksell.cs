using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Checksell
{
    public int Id { get; set; }

    public DateTime DateCreate { get; set; }

    public bool IsElectron { get; set; }

    public decimal Sum { get; set; }

    public decimal SumDiscont { get; set; }

    public decimal SumAll { get; set; }

    public int ShiftId { get; set; }

    public int? BuyerId { get; set; }

    public int TypeSell { get; set; }

    public decimal SumCash { get; set; }

    public decimal SumElectron { get; set; }

    public string? BuyerName { get; set; }

    public string? BuyerPhone { get; set; }

    public virtual ICollection<Checkgood> Checkgoods { get; set; } = new List<Checkgood>();

    public virtual Shift Shift { get; set; } = null!;
}
