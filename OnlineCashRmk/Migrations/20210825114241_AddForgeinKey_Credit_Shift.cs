using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCashRmk.Migrations
{
    public partial class AddForgeinKey_Credit_Shift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShiftId",
                table: "Credits",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Credits_ShiftId",
                table: "Credits",
                column: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Credits_Shifts_ShiftId",
                table: "Credits",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credits_Shifts_ShiftId",
                table: "Credits");

            migrationBuilder.DropIndex(
                name: "IX_Credits_ShiftId",
                table: "Credits");

            migrationBuilder.DropColumn(
                name: "ShiftId",
                table: "Credits");
        }
    }
}
