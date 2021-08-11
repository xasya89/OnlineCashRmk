using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCashRmk.Migrations
{
    public partial class AddCreditPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckGoods_Credit_CreditId",
                table: "CheckGoods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Credit",
                table: "Credit");

            migrationBuilder.RenameTable(
                name: "Credit",
                newName: "Credits");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Credits",
                table: "Credits",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CreditPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sum = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreditId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditPayments_Credits_CreditId",
                        column: x => x.CreditId,
                        principalTable: "Credits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreditPayments_CreditId",
                table: "CreditPayments",
                column: "CreditId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckGoods_Credits_CreditId",
                table: "CheckGoods",
                column: "CreditId",
                principalTable: "Credits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckGoods_Credits_CreditId",
                table: "CheckGoods");

            migrationBuilder.DropTable(
                name: "CreditPayments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Credits",
                table: "Credits");

            migrationBuilder.RenameTable(
                name: "Credits",
                newName: "Credit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Credit",
                table: "Credit",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckGoods_Credit_CreditId",
                table: "CheckGoods",
                column: "CreditId",
                principalTable: "Credit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
