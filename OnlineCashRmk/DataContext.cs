using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Models;

namespace OnlineCashRmk
{
    class DataContext:DbContext
    {
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Good> Goods { get; set; }
        public DbSet<CheckSell> CheckSells { get; set; }
        public DbSet<CheckGood> CheckGoods { get; set; }

        public DbSet<Credit> Credits { get; set; }
        public DbSet<CreditGood> CreditGoods { get; set; }
        public DbSet<CreditPayment> CreditPayments { get; set; }

        public DataContext() { }
        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=CustomerDB.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shift>().ToTable("Shifts");
            modelBuilder.Entity<Good>().ToTable("Goods");
            modelBuilder.Entity<CheckGood>().ToTable("CheckGoods");
            modelBuilder.Entity<CheckSell>().ToTable("CheckSells");

            modelBuilder.Entity<CheckGood>()
                .HasOne(c => c.Good)
                .WithMany(g => g.CheckGoods)
                .HasForeignKey(c => c.GoodId);
            modelBuilder.Entity<CheckGood>()
                .HasOne(c => c.CheckSell)
                .WithMany(s => s.CheckGoods)
                .HasForeignKey(c => c.CheckSellId);
            modelBuilder.Entity<CheckSell>()
                .HasOne(s => s.Shift)
                .WithMany(s => s.CheckSells)
                .HasForeignKey(s => s.ShiftId);

            modelBuilder.Entity<CreditGood>()
                .HasOne(ch => ch.Credit)
                .WithMany(c => c.CreditGoods)
                .HasForeignKey(ch => ch.CreditId);
            modelBuilder.Entity<CreditPayment>()
                .HasOne(p => p.Credit)
                .WithMany(c => c.CreditPayments)
                .HasForeignKey(p => p.CreditId);
            modelBuilder.Entity<Credit>()
                .HasOne(c => c.Shift)
                .WithMany(s => s.Credits)
                .HasForeignKey(c => c.ShiftId);
        }
    }
}
