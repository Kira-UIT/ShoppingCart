using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCart.Data.Migrations
{
    public partial class addtableconfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductInventories_ProductInventoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductInventoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductInventoryId",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_InvetoryId",
                table: "Products",
                column: "InvetoryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductInventories_InvetoryId",
                table: "Products",
                column: "InvetoryId",
                principalTable: "ProductInventories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductInventories_InvetoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_InvetoryId",
                table: "Products");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductInventoryId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductInventoryId",
                table: "Products",
                column: "ProductInventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductInventories_ProductInventoryId",
                table: "Products",
                column: "ProductInventoryId",
                principalTable: "ProductInventories",
                principalColumn: "Id");
        }
    }
}
