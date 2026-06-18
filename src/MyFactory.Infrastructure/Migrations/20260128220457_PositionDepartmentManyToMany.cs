using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFactory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PositionDepartmentManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM pg_constraint
                        WHERE conname = 'FK_POSITIONS_DEPARTMENTS_DepartmentEntityId'
                    ) THEN
                        ALTER TABLE "POSITIONS"
                        DROP CONSTRAINT "FK_POSITIONS_DEPARTMENTS_DepartmentEntityId";
                    END IF;
                END $$;
                """);

            migrationBuilder.DropIndex(
                name: "IX_POSITIONS_DepartmentId",
                table: "POSITIONS");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "POSITIONS");

            migrationBuilder.CreateTable(
                name: "DEPARTMENT_POSITIONS",
                columns: table => new
                {
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    PositionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEPARTMENT_POSITIONS", x => new { x.DepartmentId, x.PositionId });
                    table.ForeignKey(
                        name: "FK_DEPARTMENT_POSITIONS_DEPARTMENTS_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DEPARTMENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DEPARTMENT_POSITIONS_POSITIONS_PositionId",
                        column: x => x.PositionId,
                        principalTable: "POSITIONS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DEPARTMENT_POSITIONS_PositionId",
                table: "DEPARTMENT_POSITIONS",
                column: "PositionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POSITIONS_DEPARTMENTS_DepartmentEntityId",
                table: "POSITIONS");

            migrationBuilder.DropTable(
                name: "DEPARTMENT_POSITIONS");

            migrationBuilder.DropIndex(
                name: "IX_POSITIONS_DepartmentEntityId",
                table: "POSITIONS");

            migrationBuilder.DropColumn(
                name: "DepartmentEntityId",
                table: "POSITIONS");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "POSITIONS",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_POSITIONS_DepartmentId",
                table: "POSITIONS",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_POSITIONS_DEPARTMENTS_DepartmentId",
                table: "POSITIONS",
                column: "DepartmentId",
                principalTable: "DEPARTMENTS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
