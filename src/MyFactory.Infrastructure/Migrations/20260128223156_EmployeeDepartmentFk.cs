using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFactory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeDepartmentFk : Migration
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

            migrationBuilder.Sql("""
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM information_schema.columns
                        WHERE table_name = 'POSITIONS'
                          AND column_name = 'DepartmentEntityId'
                    ) THEN
                        ALTER TABLE "POSITIONS" DROP COLUMN "DepartmentEntityId";
                    END IF;
                END $$;
                """);

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "EMPLOYEES",
                type: "uuid",
                nullable: true);

            // Backfill DepartmentId from linked position departments (pick the first linked department)
            migrationBuilder.Sql(
                """
                UPDATE "EMPLOYEES" e
                SET "DepartmentId" = dp."DepartmentId"
                FROM "POSITIONS" p
                JOIN "DEPARTMENT_POSITIONS" dp ON dp."PositionId" = p."Id"
                WHERE e."PositionId" = p."Id" AND e."DepartmentId" IS NULL;
                """);

            // Fallback: if still NULL, assign a valid department (create default if none exists)
            migrationBuilder.Sql(
                """
                DO $$
                DECLARE dep_id uuid;
                BEGIN
                  SELECT "Id" INTO dep_id FROM "DEPARTMENTS" ORDER BY "Name" LIMIT 1;
                  IF dep_id IS NULL THEN
                    dep_id := '11111111-1111-1111-1111-111111111111';
                    INSERT INTO "DEPARTMENTS" ("Id", "Name", "Code", "Type", "IsActive", "CreatedAt")
                    VALUES (dep_id, 'Default Department', 'DEFAULT', 0, true, NOW());
                  END IF;

                  UPDATE "EMPLOYEES" e
                  SET "DepartmentId" = dep_id
                  WHERE e."DepartmentId" IS NULL;
                END $$;
                """);

            migrationBuilder.AlterColumn<Guid>(
                name: "DepartmentId",
                table: "EMPLOYEES",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EMPLOYEES_DepartmentId",
                table: "EMPLOYEES",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_EMPLOYEES_DEPARTMENTS_DepartmentId",
                table: "EMPLOYEES",
                column: "DepartmentId",
                principalTable: "DEPARTMENTS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EMPLOYEES_DEPARTMENTS_DepartmentId",
                table: "EMPLOYEES");

            migrationBuilder.DropIndex(
                name: "IX_EMPLOYEES_DepartmentId",
                table: "EMPLOYEES");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "EMPLOYEES");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentEntityId",
                table: "POSITIONS",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_POSITIONS_DEPARTMENTS_DepartmentEntityId",
                table: "POSITIONS",
                column: "DepartmentEntityId",
                principalTable: "DEPARTMENTS",
                principalColumn: "Id");
        }
    }
}
