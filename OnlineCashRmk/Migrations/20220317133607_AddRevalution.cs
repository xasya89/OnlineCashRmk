using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCashRmk.Migrations
{
    public partial class AddRevalution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Revaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Create = table.Column<DateTime>(type: "Date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revaluations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RevaluationGoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RevaluationId = table.Column<int>(type: "INTEGER", nullable: false),
                    GoodId = table.Column<int>(type: "INTEGER", nullable: false),
                    Count = table.Column<decimal>(type: "TEXT", nullable: true),
                    PriceOld = table.Column<decimal>(type: "TEXT", nullable: false),
                    PriceNew = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevaluationGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RevaluationGoods_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RevaluationGoods_Revaluations_RevaluationId",
                        column: x => x.RevaluationId,
                        principalTable: "Revaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RevaluationGoods_GoodId",
                table: "RevaluationGoods",
                column: "GoodId");

            migrationBuilder.CreateIndex(
                name: "IX_RevaluationGoods_RevaluationId",
                table: "RevaluationGoods",
                column: "RevaluationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RevaluationGoods");

            migrationBuilder.DropTable(
                name: "Revaluations");
        }
    }
}
