using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFactory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddQtyPerPackageColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "QtyPerPackage",
                table: "WAREHOUSE_MATERIALS",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql("""
                UPDATE "WAREHOUSE_MATERIALS"
                SET "QtyPerPackage" = "Qty"
                WHERE "QtyPerPackage" = 0;
            """);

            migrationBuilder.AlterColumn<decimal>(
                name: "Qty",
                table: "FINISHED_GOODS_STOCK",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<decimal>(
                name: "PackageCount",
                table: "FINISHED_GOODS_STOCK",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "QtyPerPackage",
                table: "FINISHED_GOODS_STOCK",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Qty",
                table: "FINISHED_GOODS_MOVEMENT_ITEMS",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.Sql("""
                UPDATE "FINISHED_GOODS_STOCK"
                SET "QtyPerPackage" = "Qty"
                WHERE "QtyPerPackage" = 0;
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QtyPerPackage",
                table: "WAREHOUSE_MATERIALS");

            migrationBuilder.DropColumn(
                name: "PackageCount",
                table: "FINISHED_GOODS_STOCK");

            migrationBuilder.DropColumn(
                name: "QtyPerPackage",
                table: "FINISHED_GOODS_STOCK");

            migrationBuilder.AlterColumn<int>(
                name: "Qty",
                table: "FINISHED_GOODS_STOCK",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "Qty",
                table: "FINISHED_GOODS_MOVEMENT_ITEMS",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");
        }
    }
}
