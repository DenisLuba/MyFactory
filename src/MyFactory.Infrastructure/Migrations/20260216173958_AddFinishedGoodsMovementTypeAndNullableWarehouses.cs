using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFactory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFinishedGoodsMovementTypeAndNullableWarehouses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ToWarehouseId",
                table: "FINISHED_GOODS_MOVEMENTS",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "FromWarehouseId",
                table: "FINISHED_GOODS_MOVEMENTS",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<int>(
                name: "MovementType",
                table: "FINISHED_GOODS_MOVEMENTS",
                type: "integer",
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE "FINISHED_GOODS_MOVEMENTS"
                SET "MovementType" = CASE
                    WHEN "FromWarehouseId" IS NOT NULL AND "ToWarehouseId" IS NOT NULL THEN 0
                    WHEN "FromWarehouseId" IS NOT NULL AND "ToWarehouseId" IS NULL THEN 1
                    WHEN "FromWarehouseId" IS NULL AND "ToWarehouseId" IS NOT NULL THEN 4
                    ELSE 3
                END;
                """);

            migrationBuilder.AlterColumn<int>(
                name: "MovementType",
                table: "FINISHED_GOODS_MOVEMENTS",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovementType",
                table: "FINISHED_GOODS_MOVEMENTS");

            migrationBuilder.AlterColumn<Guid>(
                name: "ToWarehouseId",
                table: "FINISHED_GOODS_MOVEMENTS",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "FromWarehouseId",
                table: "FINISHED_GOODS_MOVEMENTS",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
