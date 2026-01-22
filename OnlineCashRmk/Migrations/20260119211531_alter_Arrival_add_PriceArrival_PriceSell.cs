using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCashRmk.Migrations
{
    /// <inheritdoc />
    public partial class alter_Arrival_add_PriceArrival_PriceSell : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "ArrivalGoods",
                newName: "PriceSell");

            migrationBuilder.AddColumn<decimal>(
                name: "PriceArrival",
                table: "ArrivalGoods",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceArrival",
                table: "ArrivalGoods");

            migrationBuilder.RenameColumn(
                name: "PriceSell",
                table: "ArrivalGoods",
                newName: "Price");
        }
    }
}
