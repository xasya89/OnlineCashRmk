using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCashRmk.Migrations
{
    public partial class AddCheckTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CashierId",
                table: "Shifts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "Shifts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SumAll",
                table: "Shifts",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SumIncome",
                table: "Shifts",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SumOutcome",
                table: "Shifts",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SumSell",
                table: "Shifts",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SummReturn",
                table: "Shifts",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "CheckSells",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateCreate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsElectron = table.Column<bool>(type: "INTEGER", nullable: false),
                    Sum = table.Column<decimal>(type: "TEXT", nullable: false),
                    SumDiscont = table.Column<decimal>(type: "TEXT", nullable: false),
                    SumAll = table.Column<decimal>(type: "TEXT", nullable: false),
                    ShiftId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckSells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckSells_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckGoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Count = table.Column<double>(type: "REAL", nullable: false),
                    Cost = table.Column<decimal>(type: "TEXT", nullable: false),
                    GoodId = table.Column<int>(type: "INTEGER", nullable: false),
                    CheckSellId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckGoods_CheckSells_CheckSellId",
                        column: x => x.CheckSellId,
                        principalTable: "CheckSells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckGoods_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckGoods_CheckSellId",
                table: "CheckGoods",
                column: "CheckSellId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckGoods_GoodId",
                table: "CheckGoods",
                column: "GoodId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckSells_ShiftId",
                table: "CheckSells",
                column: "ShiftId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckGoods");

            migrationBuilder.DropTable(
                name: "CheckSells");

            migrationBuilder.DropColumn(
                name: "CashierId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "SumAll",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "SumIncome",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "SumOutcome",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "SumSell",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "SummReturn",
                table: "Shifts");
        }
    }
}
