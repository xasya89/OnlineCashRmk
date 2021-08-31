using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCashRmk.Migrations
{
    public partial class AddBarcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BarCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: true),
                    GoodId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BarCodes_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BarCodes_GoodId",
                table: "BarCodes",
                column: "GoodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BarCodes");
        }
    }
}
