using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCashRmk.Migrations
{
    public partial class AddDocSynch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocSynches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeDoc = table.Column<int>(type: "INTEGER", nullable: false),
                    DocId = table.Column<int>(type: "INTEGER", nullable: false),
                    Create = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SynchStatus = table.Column<bool>(type: "INTEGER", nullable: false),
                    Synch = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocSynches", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocSynches");
        }
    }
}
