using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFactory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFinishedGoodsScrapTableAndDescriptionPropertyToMaterialEntiry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MATERIALS",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FINISHED_GOODS_SCRAP",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductionOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    Qty = table.Column<int>(type: "integer", nullable: false),
                    ScrapDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINISHED_GOODS_SCRAP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FINISHED_GOODS_SCRAP_PRODUCTION_ORDERS_ProductionOrderId",
                        column: x => x.ProductionOrderId,
                        principalTable: "PRODUCTION_ORDERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FINISHED_GOODS_SCRAP_PRODUCTS_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PRODUCTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FINISHED_GOODS_SCRAP_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FINISHED_GOODS_SCRAP_WAREHOUSES_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "WAREHOUSES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FINISHED_GOODS_SCRAP_CreatedBy",
                table: "FINISHED_GOODS_SCRAP",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FINISHED_GOODS_SCRAP_ProductId",
                table: "FINISHED_GOODS_SCRAP",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FINISHED_GOODS_SCRAP_ProductionOrderId",
                table: "FINISHED_GOODS_SCRAP",
                column: "ProductionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_FINISHED_GOODS_SCRAP_WarehouseId",
                table: "FINISHED_GOODS_SCRAP",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FINISHED_GOODS_SCRAP");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MATERIALS");
        }
    }
}
