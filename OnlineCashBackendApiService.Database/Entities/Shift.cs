using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Shift
{
    public int Id { get; set; }

    public DateTime Start { get; set; }

    public DateTime? Stop { get; set; }

    public decimal SumAll { get; set; }

    public decimal SumSell { get; set; }

    public decimal SumIncome { get; set; }

    public decimal SumOutcome { get; set; }

    public int ShopId { get; set; }

    public int CashierId { get; set; }

    public decimal SumCreditDelivery { get; set; }

    public decimal SumCreditRepayment { get; set; }

    public decimal SumElectron { get; set; }

    public decimal SumNoElectron { get; set; }

    public Guid Uuid { get; set; }

    public decimal SumReturnCash { get; set; }

    public decimal SumReturnElectron { get; set; }

    public virtual Cashier Cashier { get; set; } = null!;

    public virtual ICollection<Checksell> Checksells { get; set; } = new List<Checksell>();

    public virtual ICollection<Shiftsale> Shiftsales { get; set; } = new List<Shiftsale>();

    public virtual Shop Shop { get; set; } = null!;
}
