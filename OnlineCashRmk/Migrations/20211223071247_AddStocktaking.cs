using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCashRmk.Migrations
{
    public partial class AddStocktaking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountPercant",
                table: "Buyers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Stocktakings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Create = table.Column<DateTime>(type: "TEXT", nullable: false),
                    isSynch = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocktakings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StocktakingGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StocktakingId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StocktakingGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StocktakingGroups_Stocktakings_StocktakingId",
                        column: x => x.StocktakingId,
                        principalTable: "Stocktakings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StocktakingGoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StocktakingGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    GoodId = table.Column<int>(type: "INTEGER", nullable: false),
                    CountFact = table.Column<decimal>(type: "TEXT", nullable: true),
                    CountDocMove = table.Column<decimal>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StocktakingGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StocktakingGoods_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StocktakingGoods_StocktakingGroups_StocktakingGroupId",
                        column: x => x.StocktakingGroupId,
                        principalTable: "StocktakingGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StocktakingGoods_GoodId",
                table: "StocktakingGoods",
                column: "GoodId");

            migrationBuilder.CreateIndex(
                name: "IX_StocktakingGoods_StocktakingGroupId",
                table: "StocktakingGoods",
                column: "StocktakingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_StocktakingGroups_StocktakingId",
                table: "StocktakingGroups",
                column: "StocktakingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StocktakingGoods");

            migrationBuilder.DropTable(
                name: "StocktakingGroups");

            migrationBuilder.DropTable(
                name: "Stocktakings");

            migrationBuilder.DropColumn(
                name: "DiscountPercant",
                table: "Buyers");
        }
    }
}
