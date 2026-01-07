using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Arrivalpayment
{
    public int Id { get; set; }

    public int ArrivalId { get; set; }

    public DateTime DatePayment { get; set; }

    public int BankAccountId { get; set; }

    public decimal Sum { get; set; }

    public virtual Arrival Arrival { get; set; } = null!;

    public virtual Bankaccount BankAccount { get; set; } = null!;
}
