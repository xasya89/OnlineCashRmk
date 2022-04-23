using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCashRmk.Migrations
{
    public partial class Alter_Buyer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountCardNum",
                table: "Buyers");

            migrationBuilder.DropColumn(
                name: "DiscountPercant",
                table: "Buyers");

            migrationBuilder.RenameColumn(
                name: "isChanged",
                table: "Buyers",
                newName: "TemporyPercent");

            migrationBuilder.RenameColumn(
                name: "DiscountType",
                table: "Buyers",
                newName: "SpecialPercent");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountSum",
                table: "Buyers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TemporyPercent",
                table: "Buyers",
                newName: "isChanged");

            migrationBuilder.RenameColumn(
                name: "SpecialPercent",
                table: "Buyers",
                newName: "DiscountType");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountSum",
                table: "Buyers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "DiscountCardNum",
                table: "Buyers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountPercant",
                table: "Buyers",
                type: "INTEGER",
                nullable: true);
        }
    }
}
