using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFactory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductionStageAssignmentsAndSewingOperationLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssignmentId",
                table: "SEWING_OPERATIONS",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "PRODUCTION_ORDER_DEPARTMENT_EMPLOYEES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductionOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Stage = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkDate = table.Column<DateOnly>(type: "date", nullable: false),
                    AssignedQty = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CompletedQty = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTION_ORDER_DEPARTMENT_EMPLOYEES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRODUCTION_ORDER_DEPARTMENT_EMPLOYEES_DEPARTMENTS_Departmen~",
                        column: x => x.DepartmentId,
                        principalTable: "DEPARTMENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PRODUCTION_ORDER_DEPARTMENT_EMPLOYEES_EMPLOYEES_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "EMPLOYEES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PRODUCTION_ORDER_DEPARTMENT_EMPLOYEES_PRODUCTION_ORDERS_Pro~",
                        column: x => x.ProductionOrderId,
                        principalTable: "PRODUCTION_ORDERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SEWING_OPERATIONS_AssignmentId",
                table: "SEWING_OPERATIONS",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTION_ORDER_DEPARTMENT_EMPLOYEES_DepartmentId",
                table: "PRODUCTION_ORDER_DEPARTMENT_EMPLOYEES",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTION_ORDER_DEPARTMENT_EMPLOYEES_EmployeeId",
                table: "PRODUCTION_ORDER_DEPARTMENT_EMPLOYEES",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTION_ORDER_DEPARTMENT_EMPLOYEES_ProductionOrderId_Sta~",
                table: "PRODUCTION_ORDER_DEPARTMENT_EMPLOYEES",
                columns: new[] { "ProductionOrderId", "Stage", "EmployeeId", "WorkDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_SEWING_OPERATIONS_PRODUCTION_ORDER_DEPARTMENT_EMPLOYEES_Ass~",
                table: "SEWING_OPERATIONS",
                column: "AssignmentId",
                principalTable: "PRODUCTION_ORDER_DEPARTMENT_EMPLOYEES",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SEWING_OPERATIONS_PRODUCTION_ORDER_DEPARTMENT_EMPLOYEES_Ass~",
                table: "SEWING_OPERATIONS");

            migrationBuilder.DropTable(
                name: "PRODUCTION_ORDER_DEPARTMENT_EMPLOYEES");

            migrationBuilder.DropIndex(
                name: "IX_SEWING_OPERATIONS_AssignmentId",
                table: "SEWING_OPERATIONS");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "SEWING_OPERATIONS");
        }
    }
}
