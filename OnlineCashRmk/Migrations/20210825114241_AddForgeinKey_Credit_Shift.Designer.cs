﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineCashRmk;

namespace OnlineCashRmk.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20210825114241_AddForgeinKey_Credit_Shift")]
    partial class AddForgeinKey_Credit_Shift
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("OnlineCashRmk.Models.CheckGood", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CheckSellId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Cost")
                        .HasColumnType("TEXT");

                    b.Property<double>("Count")
                        .HasColumnType("REAL");

                    b.Property<int>("GoodId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CheckSellId");

                    b.HasIndex("GoodId");

                    b.ToTable("CheckGoods");
                });

            modelBuilder.Entity("OnlineCashRmk.Models.CheckSell", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsElectron")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ShiftId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Sum")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("SumAll")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("SumDiscont")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ShiftId");

                    b.ToTable("CheckSells");
                });

            modelBuilder.Entity("OnlineCashRmk.Models.Credit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Creditor")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ShiftId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Sum")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("SumAll")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("SumCredit")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("SumDiscont")
                        .HasColumnType("TEXT");

                    b.Property<bool>("isSynch")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ShiftId");

                    b.ToTable("Credits");
                });

            modelBuilder.Entity("OnlineCashRmk.Models.CreditGood", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Cost")
                        .HasColumnType("TEXT");

                    b.Property<double>("Count")
                        .HasColumnType("REAL");

                    b.Property<int>("CreditId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GoodId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CreditId");

                    b.HasIndex("GoodId");

                    b.ToTable("CreditGoods");
                });

            modelBuilder.Entity("OnlineCashRmk.Models.CreditPayment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CreditId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DatePayment")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Sum")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreditId");

                    b.ToTable("CreditPayments");
                });

            modelBuilder.Entity("OnlineCashRmk.Models.Good", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Article")
                        .HasColumnType("TEXT");

                    b.Property<string>("BarCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<int>("Unit")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Goods");
                });

            modelBuilder.Entity("OnlineCashRmk.Models.Shift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CashierId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ShopId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Start")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Stop")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("SumAll")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("SumIncome")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("SumOutcome")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("SumSell")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("SummReturn")
                        .HasColumnType("TEXT");

                    b.Property<bool>("isSynch")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Shifts");
                });

            modelBuilder.Entity("OnlineCashRmk.Models.CheckGood", b =>
                {
                    b.HasOne("OnlineCashRmk.Models.CheckSell", "CheckSell")
                        .WithMany("CheckGoods")
                        .HasForeignKey("CheckSellId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineCashRmk.Models.Good", "Good")
                        .WithMany("CheckGoods")
                        .HasForeignKey("GoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CheckSell");

                    b.Navigation("Good");
                });

            modelBuilder.Entity("OnlineCashRmk.Models.CheckSell", b =>
                {
                    b.HasOne("OnlineCashRmk.Models.Shift", "Shift")
                        .WithMany("CheckSells")
                        .HasForeignKey("ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shift");
                });

            modelBuilder.Entity("OnlineCashRmk.Models.Credit", b =>
                {
                    b.HasOne("OnlineCashRmk.Models.Shift", "Shift")
                        .WithMany("Credits")
                        .HasForeignKey("ShiftId");

                    b.Navigation("Shift");
                });

            modelBuilder.Entity("OnlineCashRmk.Models.CreditGood", b =>
                {
                    b.HasOne("OnlineCashRmk.Models.Credit", "Credit")
                        .WithMany("CreditGoods")
                        .HasForeignKey("CreditId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineCashRmk.Models.Good", "Good")
                        .WithMany()
                        .HasForeignKey("GoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Credit");

                    b.Navigation("Good");
                });

            modelBuilder.Entity("OnlineCashRmk.Models.CreditPayment", b =>
                {
                    b.HasOne("OnlineCashRmk.Models.Credit", "Credit")
                        .WithMany("CreditPayments")
                        .HasForeignKey("CreditId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Credit");
                });

            modelBuilder.Entity("OnlineCashRmk.Models.CheckSell", b =>
                {
                    b.Navigation("CheckGoods");
                });

            modelBuilder.Entity("OnlineCashRmk.Models.Credit", b =>
                {
                    b.Navigation("CreditGoods");

                    b.Navigation("CreditPayments");
                });

            modelBuilder.Entity("OnlineCashRmk.Models.Good", b =>
                {
                    b.Navigation("CheckGoods");
                });

            modelBuilder.Entity("OnlineCashRmk.Models.Shift", b =>
                {
                    b.Navigation("CheckSells");

                    b.Navigation("Credits");
                });
#pragma warning restore 612, 618
        }
    }
}
