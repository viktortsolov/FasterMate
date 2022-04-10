using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FasterMate.Infrastrucutre.Data.Migrations
{
    public partial class AddOwnerOfTheGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileId",
                table: "Groups",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ProfileId",
                table: "Groups",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Profiles_ProfileId",
                table: "Groups",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Profiles_ProfileId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_ProfileId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Groups");
        }
    }
}
