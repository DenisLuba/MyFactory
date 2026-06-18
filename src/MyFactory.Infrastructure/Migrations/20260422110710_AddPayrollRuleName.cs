using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFactory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPayrollRuleName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PAYROLL_RULES",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "PAYROLL_RULES");
        }
    }
}
