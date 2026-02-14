using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCashRmk.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Shift_Alter_SumReturn_columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SummReturn",
                table: "Shifts",
                newName: "SumReturnElectron");

            migrationBuilder.AddColumn<decimal>(
                name: "SumReturnCash",
                table: "Shifts",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SumReturnCash",
                table: "Shifts");

            migrationBuilder.RenameColumn(
                name: "SumReturnElectron",
                table: "Shifts",
                newName: "SummReturn");
        }
    }
}
