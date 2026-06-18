using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFactory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateProductTypesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductTypeId",
                table: "PRODUCTS",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PRODUCT_TYPES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT_TYPES", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTS_ProductTypeId",
                table: "PRODUCTS",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_TYPES_Type",
                table: "PRODUCT_TYPES",
                column: "Type",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PRODUCTS_PRODUCT_TYPES_ProductTypeId",
                table: "PRODUCTS",
                column: "ProductTypeId",
                principalTable: "PRODUCT_TYPES",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PRODUCTS_PRODUCT_TYPES_ProductTypeId",
                table: "PRODUCTS");

            migrationBuilder.DropTable(
                name: "PRODUCT_TYPES");

            migrationBuilder.DropIndex(
                name: "IX_PRODUCTS_ProductTypeId",
                table: "PRODUCTS");

            migrationBuilder.DropColumn(
                name: "ProductTypeId",
                table: "PRODUCTS");
        }
    }
}
