using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBook.Migrations
{
    public partial class updateTableMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "SeoKeywords",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "SeoTitle",
                table: "Menus");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "Position",
                table: "Menus",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Menus",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Menus",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_ApplicationUserId",
                table: "Menus",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_AspNetUsers_ApplicationUserId",
                table: "Menus",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_AspNetUsers_ApplicationUserId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_ApplicationUserId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Menus");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Position",
                table: "Menus",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Menus",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoKeywords",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoTitle",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
