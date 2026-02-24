using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCashRmk.Migrations
{
    /// <inheritdoc />
    public partial class Drop_Columns_In_Buyers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "Buyers");

            migrationBuilder.DropColumn(
                name: "DiscountSum",
                table: "Buyers");

            migrationBuilder.DropColumn(
                name: "SumBuy",
                table: "Buyers");

            migrationBuilder.DropColumn(
                name: "TemporyPercent",
                table: "Buyers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Birthday",
                table: "Buyers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountSum",
                table: "Buyers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SumBuy",
                table: "Buyers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TemporyPercent",
                table: "Buyers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
