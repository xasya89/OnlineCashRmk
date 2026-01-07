using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Stocktakinggroup
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int StocktakingId { get; set; }

    public virtual Stocktaking Stocktaking { get; set; } = null!;

    public virtual ICollection<Stocktakinggood> Stocktakinggoods { get; set; } = new List<Stocktakinggood>();
}
