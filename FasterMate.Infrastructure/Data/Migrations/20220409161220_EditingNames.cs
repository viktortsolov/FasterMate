using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FasterMate.Infrastrucutre.Data.Migrations
{
    public partial class EditingNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMemebers_Groups_GroupId",
                table: "GroupMemebers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMemebers_Profiles_ProfileId",
                table: "GroupMemebers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupMemebers",
                table: "GroupMemebers");

            migrationBuilder.RenameTable(
                name: "GroupMemebers",
                newName: "GroupMembers");

            migrationBuilder.RenameIndex(
                name: "IX_GroupMemebers_ProfileId",
                table: "GroupMembers",
                newName: "IX_GroupMembers_ProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupMembers",
                table: "GroupMembers",
                columns: new[] { "GroupId", "ProfileId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_Groups_GroupId",
                table: "GroupMembers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_Profiles_ProfileId",
                table: "GroupMembers",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_Groups_GroupId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_Profiles_ProfileId",
                table: "GroupMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupMembers",
                table: "GroupMembers");

            migrationBuilder.RenameTable(
                name: "GroupMembers",
                newName: "GroupMemebers");

            migrationBuilder.RenameIndex(
                name: "IX_GroupMembers_ProfileId",
                table: "GroupMemebers",
                newName: "IX_GroupMemebers_ProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupMemebers",
                table: "GroupMemebers",
                columns: new[] { "GroupId", "ProfileId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMemebers_Groups_GroupId",
                table: "GroupMemebers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMemebers_Profiles_ProfileId",
                table: "GroupMemebers",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
