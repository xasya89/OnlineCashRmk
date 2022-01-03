using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCashRmk.Migrations
{
    public partial class AddCheckPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CheckSellId = table.Column<int>(type: "INTEGER", nullable: false),
                    TypePayment = table.Column<int>(type: "INTEGER", nullable: false),
                    Income = table.Column<decimal>(type: "TEXT", nullable: false),
                    Sum = table.Column<decimal>(type: "TEXT", nullable: false),
                    Retturn = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckPayments_CheckSells_CheckSellId",
                        column: x => x.CheckSellId,
                        principalTable: "CheckSells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckPayments_CheckSellId",
                table: "CheckPayments",
                column: "CheckSellId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckPayments");
        }
    }
}
