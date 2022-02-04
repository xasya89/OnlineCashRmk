using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCashRmk.Migrations
{
    public partial class AlterArrival_AddExpires : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresDate",
                table: "ArrivalGoods",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiresDate",
                table: "ArrivalGoods");
        }
    }
}
