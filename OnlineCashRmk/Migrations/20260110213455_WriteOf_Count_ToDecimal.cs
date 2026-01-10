using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCashRmk.Migrations
{
    /// <inheritdoc />
    public partial class WriteOf_Count_ToDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Count",
                table: "WriteofGoods",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Count",
                table: "WriteofGoods",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");
        }
    }
}
