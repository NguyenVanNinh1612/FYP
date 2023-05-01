using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBook.Migrations
{
    public partial class updateTableReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId1",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "Reviews",
                newName: "ApplicationUserId1");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reviews",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_UserId1",
                table: "Reviews",
                newName: "IX_Reviews_ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_ApplicationUserId1",
                table: "Reviews",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_ApplicationUserId1",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId1",
                table: "Reviews",
                newName: "UserId1");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Reviews",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ApplicationUserId1",
                table: "Reviews",
                newName: "IX_Reviews_UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId1",
                table: "Reviews",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
