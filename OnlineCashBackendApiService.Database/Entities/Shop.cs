using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Shop
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Adress { get; set; }

    public string? Inn { get; set; }

    public string? OrgName { get; set; }

    public virtual ICollection<Arrival> Arrivals { get; set; } = new List<Arrival>();

    public virtual ICollection<Cashmoney> Cashmoneys { get; set; } = new List<Cashmoney>();

    public virtual ICollection<Goodbalancehistory> Goodbalancehistories { get; set; } = new List<Goodbalancehistory>();

    public virtual ICollection<Goodbalance> Goodbalances { get; set; } = new List<Goodbalance>();

    public virtual ICollection<Goodprice> Goodprices { get; set; } = new List<Goodprice>();

    public virtual ICollection<Moneybalancehistory> Moneybalancehistories { get; set; } = new List<Moneybalancehistory>();

    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();

    public virtual ICollection<Stocktaking> Stocktakings { get; set; } = new List<Stocktaking>();

    public virtual ICollection<Sumbalancehistory> Sumbalancehistories { get; set; } = new List<Sumbalancehistory>();

    public virtual ICollection<Writeof> Writeofs { get; set; } = new List<Writeof>();
}
