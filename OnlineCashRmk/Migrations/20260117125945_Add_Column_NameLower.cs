using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCashRmk.Migrations
{
    /// <inheritdoc />
    public partial class Add_Column_NameLower : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameLower",
                table: "Goods",
                type: "TEXT",
                nullable: true);
            migrationBuilder.Sql("CREATE INDEX IX_Goods_NameLower ON Goods (NameLower) WHERE IsDeleted = 0;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameLower",
                table: "Goods");
        }
    }
}
