using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogApp.Migrations
{
    public partial class edithistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_editHistories_AspNetUsers_AuthorId",
                table: "editHistories");

            migrationBuilder.DropIndex(
                name: "IX_editHistories_AuthorId",
                table: "editHistories");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "editHistories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "editHistories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_editHistories_AuthorId",
                table: "editHistories",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_editHistories_AspNetUsers_AuthorId",
                table: "editHistories",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
