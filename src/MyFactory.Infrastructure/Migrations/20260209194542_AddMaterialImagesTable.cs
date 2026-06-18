using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFactory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialImagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MATERIAL_IMAGES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MATERIAL_IMAGES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MATERIAL_IMAGES_MATERIALS_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "MATERIALS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MATERIAL_IMAGES_MaterialId",
                table: "MATERIAL_IMAGES",
                column: "MaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MATERIAL_IMAGES");
        }
    }
}
