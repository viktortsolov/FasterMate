using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FasterMate.Infrastrucutre.Data.Migrations
{
    public partial class FinalMigrationMessageNameColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateOn",
                table: "Messages",
                newName: "CreatedOn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Messages",
                newName: "CreateOn");
        }
    }
}
