using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCashRmk.Migrations
{
    public partial class AlterRevaluation_AddUuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                table: "Revaluations",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "Revaluations");
        }
    }
}
