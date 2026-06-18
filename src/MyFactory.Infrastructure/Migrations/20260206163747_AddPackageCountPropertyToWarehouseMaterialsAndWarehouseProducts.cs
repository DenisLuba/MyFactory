using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFactory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPackageCountPropertyToWarehouseMaterialsAndWarehouseProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PackageCount",
                table: "WAREHOUSE_PRODUCTS",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PackageCount",
                table: "WAREHOUSE_MATERIALS",
                type: "numeric(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageCount",
                table: "WAREHOUSE_PRODUCTS");

            migrationBuilder.DropColumn(
                name: "PackageCount",
                table: "WAREHOUSE_MATERIALS");
        }
    }
}
