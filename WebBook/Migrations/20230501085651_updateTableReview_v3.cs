using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBook.Migrations
{
    public partial class updateTableReview_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_ApplicationUserId1",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ApplicationUserId1",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Reviews");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ApplicationUserId",
                table: "Reviews",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_ApplicationUserId",
                table: "Reviews",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_ApplicationUserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ApplicationUserId",
                table: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "Reviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ApplicationUserId1",
                table: "Reviews",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_ApplicationUserId1",
                table: "Reviews",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
