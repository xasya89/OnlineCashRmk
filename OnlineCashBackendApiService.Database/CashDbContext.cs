using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OnlineCashBackendApiService.Database.Entities;

namespace OnlineCashBackendApiService.Database;

public partial class CashDbContext : DbContext
{
    public CashDbContext()
    {
    }

    public CashDbContext(DbContextOptions<CashDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Arrival> Arrivals { get; set; }

    public virtual DbSet<Arrivalgood> Arrivalgoods { get; set; }

    public virtual DbSet<Arrivalpayment> Arrivalpayments { get; set; }

    public virtual DbSet<Arrivalphoto> Arrivalphotos { get; set; }

    public virtual DbSet<Bankaccount> Bankaccounts { get; set; }

    public virtual DbSet<Barcode> Barcodes { get; set; }

    public virtual DbSet<Cashier> Cashiers { get; set; }

    public virtual DbSet<Cashmoney> Cashmoneys { get; set; }

    public virtual DbSet<Checkgood> Checkgoods { get; set; }

    public virtual DbSet<Checksell> Checksells { get; set; }

    public virtual DbSet<Docsynch> Docsynches { get; set; }

    public virtual DbSet<Documenthistory> Documenthistories { get; set; }

    public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }

    public virtual DbSet<Good> Goods { get; set; }

    public virtual DbSet<Goodadded> Goodaddeds { get; set; }

    public virtual DbSet<Goodbalance> Goodbalances { get; set; }

    public virtual DbSet<Goodbalancehistory> Goodbalancehistories { get; set; }

    public virtual DbSet<Goodcountbalance> Goodcountbalances { get; set; }

    public virtual DbSet<Goodcountbalancecurrent> Goodcountbalancecurrents { get; set; }

    public virtual DbSet<Goodcountdochistory> Goodcountdochistories { get; set; }

    public virtual DbSet<Goodgroup> Goodgroups { get; set; }

    public virtual DbSet<Goodprice> Goodprices { get; set; }

    public virtual DbSet<Moneybalancehistory> Moneybalancehistories { get; set; }

    public virtual DbSet<Movegood> Movegoods { get; set; }

    public virtual DbSet<Newgoodfromcash> Newgoodfromcashes { get; set; }

    public virtual DbSet<PriceControll> PriceControlls { get; set; }

    public virtual DbSet<PriceControllPosition> PriceControllPositions { get; set; }

    public virtual DbSet<Reportsafterstocktaking> Reportsafterstocktakings { get; set; }

    public virtual DbSet<Revaluation> Revaluations { get; set; }

    public virtual DbSet<Revaluationgood> Revaluationgoods { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<Shiftsale> Shiftsales { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<Stocktaking> Stocktakings { get; set; }

    public virtual DbSet<Stocktakinggood> Stocktakinggoods { get; set; }

    public virtual DbSet<Stocktakinggroup> Stocktakinggroups { get; set; }

    public virtual DbSet<Stocktakingsummarygood> Stocktakingsummarygoods { get; set; }

    public virtual DbSet<Sumbalancehistory> Sumbalancehistories { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Telegramuser> Telegramusers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Writeof> Writeofs { get; set; }

    public virtual DbSet<Writeofgood> Writeofgoods { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
