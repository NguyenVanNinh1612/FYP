using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBook.Migrations
{
    public partial class updateTablePost_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Menus_MenuId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_MenuId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Posts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "Posts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Posts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_MenuId",
                table: "Posts",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Menus_MenuId",
                table: "Posts",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id");
        }
    }
}
