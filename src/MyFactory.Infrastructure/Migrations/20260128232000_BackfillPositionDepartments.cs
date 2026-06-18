using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFactory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BackfillPositionDepartments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Backfill missing department-position links from employees
            migrationBuilder.Sql(
                """
                INSERT INTO "DEPARTMENT_POSITIONS" ("DepartmentId", "PositionId")
                SELECT DISTINCT e."DepartmentId", e."PositionId"
                FROM "EMPLOYEES" e
                LEFT JOIN "DEPARTMENT_POSITIONS" dp
                  ON dp."DepartmentId" = e."DepartmentId" AND dp."PositionId" = e."PositionId"
                WHERE dp."PositionId" IS NULL;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove links that match employees table (best-effort rollback)
            migrationBuilder.Sql(
                """
                DELETE FROM "DEPARTMENT_POSITIONS" dp
                USING "EMPLOYEES" e
                WHERE dp."DepartmentId" = e."DepartmentId" AND dp."PositionId" = e."PositionId";
                """);
        }
    }
}
