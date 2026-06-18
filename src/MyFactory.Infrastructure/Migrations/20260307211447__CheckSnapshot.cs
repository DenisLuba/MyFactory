using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFactory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _CheckSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderNumber",
                table: "SALES_ORDERS",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('\"OrderNumberSequence\"')",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "NEXT VALUE FOR OrderNumberSequence");

            migrationBuilder.AlterColumn<int>(
                name: "ProductionOrderNumber",
                table: "PRODUCTION_ORDERS",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('\"ProductionOrderNumberSequence\"')",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "NEXT VALUE FOR ProductionOrderNumberSequence");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderNumber",
                table: "SALES_ORDERS",
                type: "integer",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR OrderNumberSequence",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "nextval('\"OrderNumberSequence\"')");

            migrationBuilder.AlterColumn<int>(
                name: "ProductionOrderNumber",
                table: "PRODUCTION_ORDERS",
                type: "integer",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR ProductionOrderNumberSequence",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "nextval('\"ProductionOrderNumberSequence\"')");
        }
    }
}
