using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFactory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PurchaseNumColumnInMaterialPurchases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PurchaseNumber",
                table: "MATERIAL_PURCHASE_ORDERS",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_MATERIAL_PURCHASE_ORDERS_PurchaseNumber",
                table: "MATERIAL_PURCHASE_ORDERS",
                column: "PurchaseNumber",
                unique: true,
                filter: "\"PurchaseNumber\" > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MATERIAL_PURCHASE_ORDERS_PurchaseNumber",
                table: "MATERIAL_PURCHASE_ORDERS");

            migrationBuilder.DropColumn(
                name: "PurchaseNumber",
                table: "MATERIAL_PURCHASE_ORDERS");
        }
    }
}
