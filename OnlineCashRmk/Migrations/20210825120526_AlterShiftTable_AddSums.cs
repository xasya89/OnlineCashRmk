using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCashRmk.Migrations
{
    public partial class AlterShiftTable_AddSums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SumCredit",
                table: "Shifts",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SumElectron",
                table: "Shifts",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SumNoElectron",
                table: "Shifts",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SumCredit",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "SumElectron",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "SumNoElectron",
                table: "Shifts");
        }
    }
}
