using System;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Database.Entities;

public partial class Good
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Article { get; set; }

    public string? BarCode { get; set; }

    public int Unit { get; set; }

    public decimal Price { get; set; }

    public Guid Uuid { get; set; }

    public int? GoodGroupId { get; set; }

    public int GroupId { get; set; }

    public int? SupplierId { get; set; }

    public int SpecialType { get; set; }

    public double? Vpackage { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Arrivalgood> Arrivalgoods { get; set; } = new List<Arrivalgood>();

    public virtual ICollection<Barcode> Barcodes { get; set; } = new List<Barcode>();

    public virtual ICollection<Checkgood> Checkgoods { get; set; } = new List<Checkgood>();

    public virtual Goodgroup? GoodGroup { get; set; }

    public virtual ICollection<Goodadded> Goodaddeds { get; set; } = new List<Goodadded>();

    public virtual ICollection<Goodcountbalancecurrent> Goodcountbalancecurrents { get; set; } = new List<Goodcountbalancecurrent>();

    public virtual ICollection<Goodcountbalance> Goodcountbalances { get; set; } = new List<Goodcountbalance>();

    public virtual ICollection<Goodcountdochistory> Goodcountdochistories { get; set; } = new List<Goodcountdochistory>();

    public virtual ICollection<Goodprice> Goodprices { get; set; } = new List<Goodprice>();

    public virtual ICollection<Movegood> Movegoods { get; set; } = new List<Movegood>();

    public virtual ICollection<Newgoodfromcash> Newgoodfromcashes { get; set; } = new List<Newgoodfromcash>();

    public virtual ICollection<PriceControllPosition> PriceControllPositions { get; set; } = new List<PriceControllPosition>();

    public virtual ICollection<PriceControll> PriceControlls { get; set; } = new List<PriceControll>();

    public virtual ICollection<Revaluationgood> Revaluationgoods { get; set; } = new List<Revaluationgood>();

    public virtual ICollection<Shiftsale> Shiftsales { get; set; } = new List<Shiftsale>();

    public virtual ICollection<Stocktakinggood> Stocktakinggoods { get; set; } = new List<Stocktakinggood>();

    public virtual ICollection<Stocktakingsummarygood> Stocktakingsummarygoods { get; set; } = new List<Stocktakingsummarygood>();

    public virtual Supplier? Supplier { get; set; }

    public virtual ICollection<Writeofgood> Writeofgoods { get; set; } = new List<Writeofgood>();
}
