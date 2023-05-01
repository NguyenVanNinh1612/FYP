using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBook.Migrations
{
    public partial class udpateTablePost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Menus_MenuId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "SeoKeywords",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "SeoTitle",
                table: "Posts");

            migrationBuilder.AlterColumn<int>(
                name: "MenuId",
                table: "Posts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ApplicationUserId",
                table: "Posts",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_ApplicationUserId",
                table: "Posts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Menus_MenuId",
                table: "Posts",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_ApplicationUserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Menus_MenuId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ApplicationUserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Posts");

            migrationBuilder.AlterColumn<int>(
                name: "MenuId",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoKeywords",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoTitle",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Menus_MenuId",
                table: "Posts",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
