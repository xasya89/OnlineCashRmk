using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCashRmk.Migrations
{
    public partial class AddArrivals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Inn = table.Column<string>(type: "TEXT", nullable: true),
                    Kpp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Arrivals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Num = table.Column<int>(type: "INTEGER", nullable: false),
                    DateArrival = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SupplierId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arrivals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Arrivals_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArrivalGoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ArrivalId = table.Column<int>(type: "INTEGER", nullable: false),
                    GoodId = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Count = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArrivalGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArrivalGoods_Arrivals_ArrivalId",
                        column: x => x.ArrivalId,
                        principalTable: "Arrivals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArrivalGoods_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArrivalGoods_ArrivalId",
                table: "ArrivalGoods",
                column: "ArrivalId");

            migrationBuilder.CreateIndex(
                name: "IX_ArrivalGoods_GoodId",
                table: "ArrivalGoods",
                column: "GoodId");

            migrationBuilder.CreateIndex(
                name: "IX_Arrivals_SupplierId",
                table: "Arrivals",
                column: "SupplierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArrivalGoods");

            migrationBuilder.DropTable(
                name: "Arrivals");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
