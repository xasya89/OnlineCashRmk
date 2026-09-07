using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCashRmk.Migrations
{
    /// <inheritdoc />
    public partial class AddGoodPriceOld : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PriceChangedDate",
                table: "Goods",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceOld",
                table: "Goods",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceChangedDate",
                table: "Goods");

            migrationBuilder.DropColumn(
                name: "PriceOld",
                table: "Goods");
        }
    }
}
