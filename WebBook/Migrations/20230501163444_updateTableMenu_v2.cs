using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBook.Migrations
{
    public partial class updateTableMenu_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_AspNetUsers_ApplicationUserId",
                table: "Menus");

            migrationBuilder.DropForeignKey(
                name: "FK_News_Menus_MenuId",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_MenuId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "News");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Menus",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_AspNetUsers_ApplicationUserId",
                table: "Menus",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_AspNetUsers_ApplicationUserId",
                table: "Menus");

            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "News",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Menus",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_News_MenuId",
                table: "News",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_AspNetUsers_ApplicationUserId",
                table: "Menus",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_News_Menus_MenuId",
                table: "News",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id");
        }
    }
}
