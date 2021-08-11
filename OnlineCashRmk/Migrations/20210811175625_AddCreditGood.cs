using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCashRmk.Migrations
{
    public partial class AddCreditGood : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckGoods_Credits_CreditId",
                table: "CheckGoods");

            migrationBuilder.DropIndex(
                name: "IX_CheckGoods_CreditId",
                table: "CheckGoods");

            migrationBuilder.DropColumn(
                name: "CreditId",
                table: "CheckGoods");

            migrationBuilder.CreateTable(
                name: "CreditGoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Count = table.Column<double>(type: "REAL", nullable: false),
                    Cost = table.Column<decimal>(type: "TEXT", nullable: false),
                    GoodId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreditId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditGoods_Credits_CreditId",
                        column: x => x.CreditId,
                        principalTable: "Credits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditGoods_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreditGoods_CreditId",
                table: "CreditGoods",
                column: "CreditId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditGoods_GoodId",
                table: "CreditGoods",
                column: "GoodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditGoods");

            migrationBuilder.AddColumn<int>(
                name: "CreditId",
                table: "CheckGoods",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CheckGoods_CreditId",
                table: "CheckGoods",
                column: "CreditId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckGoods_Credits_CreditId",
                table: "CheckGoods",
                column: "CreditId",
                principalTable: "Credits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
