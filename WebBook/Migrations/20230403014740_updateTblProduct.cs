using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBook.Migrations
{
    public partial class updateTblProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Prodcts_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Prodcts_Categories_ProductCategoryId",
                table: "Prodcts");

            migrationBuilder.DropForeignKey(
                name: "FK_Prodcts_Suppliers_SupplierId",
                table: "Prodcts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prodcts",
                table: "Prodcts");

            migrationBuilder.RenameTable(
                name: "Prodcts",
                newName: "Products");

            migrationBuilder.RenameIndex(
                name: "IX_Prodcts_SupplierId",
                table: "Products",
                newName: "IX_Products_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Prodcts_ProductCategoryId",
                table: "Products",
                newName: "IX_Products_ProductCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                table: "Products",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_ProductCategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Prodcts");

            migrationBuilder.RenameIndex(
                name: "IX_Products_SupplierId",
                table: "Prodcts",
                newName: "IX_Prodcts_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ProductCategoryId",
                table: "Prodcts",
                newName: "IX_Prodcts_ProductCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prodcts",
                table: "Prodcts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Prodcts_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Prodcts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prodcts_Categories_ProductCategoryId",
                table: "Prodcts",
                column: "ProductCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prodcts_Suppliers_SupplierId",
                table: "Prodcts",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }
    }
}
