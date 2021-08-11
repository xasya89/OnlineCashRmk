using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCashRmk.Migrations
{
    public partial class admigrationAddCreit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreditId",
                table: "CheckGoods",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Credit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Creditor = table.Column<string>(type: "TEXT", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Sum = table.Column<decimal>(type: "TEXT", nullable: false),
                    SumDiscont = table.Column<decimal>(type: "TEXT", nullable: false),
                    SumAll = table.Column<decimal>(type: "TEXT", nullable: false),
                    isSynch = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credit", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckGoods_CreditId",
                table: "CheckGoods",
                column: "CreditId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckGoods_Credit_CreditId",
                table: "CheckGoods",
                column: "CreditId",
                principalTable: "Credit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckGoods_Credit_CreditId",
                table: "CheckGoods");

            migrationBuilder.DropTable(
                name: "Credit");

            migrationBuilder.DropIndex(
                name: "IX_CheckGoods_CreditId",
                table: "CheckGoods");

            migrationBuilder.DropColumn(
                name: "CreditId",
                table: "CheckGoods");
        }
    }
}
