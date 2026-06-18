using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFactory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeOrderNumbersIntV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // SALES_ORDERS.OrderNumber -> int + sequence default
            migrationBuilder.Sql("""
                ALTER TABLE "SALES_ORDERS"
                ALTER COLUMN "OrderNumber" DROP DEFAULT;
                """);

            migrationBuilder.Sql("""
                ALTER TABLE "SALES_ORDERS"
                ALTER COLUMN "OrderNumber" TYPE integer
                USING (
                    CASE
                        WHEN "OrderNumber" IS NULL OR btrim("OrderNumber"::text) = '' THEN 0
                        ELSE "OrderNumber"::integer
                    END
                );
                """);

            migrationBuilder.Sql("""
                CREATE SEQUENCE IF NOT EXISTS "OrderNumberSequence"
                AS integer START WITH 1 INCREMENT BY 1;
                """);

            migrationBuilder.Sql("""
                ALTER TABLE "SALES_ORDERS"
                ALTER COLUMN "OrderNumber" SET DEFAULT nextval('"OrderNumberSequence"');
                """);

            migrationBuilder.Sql("""
                SELECT setval(
                    '"OrderNumberSequence"',
                    GREATEST(COALESCE((SELECT MAX("OrderNumber") FROM "SALES_ORDERS"), 0) + 1, 1),
                    false
                );
                """);

            // PRODUCTION_ORDERS.ProductionOrderNumber -> int + sequence default
            migrationBuilder.Sql("""
                ALTER TABLE "PRODUCTION_ORDERS"
                ALTER COLUMN "ProductionOrderNumber" DROP DEFAULT;
                """);

            migrationBuilder.Sql("""
                ALTER TABLE "PRODUCTION_ORDERS"
                ALTER COLUMN "ProductionOrderNumber" TYPE integer
                USING (
                    CASE
                        WHEN "ProductionOrderNumber" IS NULL OR btrim("ProductionOrderNumber"::text) = '' THEN 0
                        ELSE "ProductionOrderNumber"::integer
                    END
                );
                """);

            migrationBuilder.Sql("""
                CREATE SEQUENCE IF NOT EXISTS "ProductionOrderNumberSequence"
                AS integer START WITH 1 INCREMENT BY 1;
                """);

            migrationBuilder.Sql("""
                ALTER TABLE "PRODUCTION_ORDERS"
                ALTER COLUMN "ProductionOrderNumber" SET DEFAULT nextval('"ProductionOrderNumberSequence"');
                """);

            migrationBuilder.Sql("""
                SELECT setval(
                    '"ProductionOrderNumberSequence"',
                    GREATEST(COALESCE((SELECT MAX("ProductionOrderNumber") FROM "PRODUCTION_ORDERS"), 0) + 1, 1),
                    false
                );
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "SALES_ORDERS"
                ALTER COLUMN "OrderNumber" DROP DEFAULT;
                """);

            migrationBuilder.Sql("""
                ALTER TABLE "SALES_ORDERS"
                ALTER COLUMN "OrderNumber" TYPE text USING "OrderNumber"::text;
                """);

            migrationBuilder.Sql("""DROP SEQUENCE IF EXISTS "OrderNumberSequence";""");

            migrationBuilder.Sql("""
                ALTER TABLE "PRODUCTION_ORDERS"
                ALTER COLUMN "ProductionOrderNumber" DROP DEFAULT;
                """);

            migrationBuilder.Sql("""
                ALTER TABLE "PRODUCTION_ORDERS"
                ALTER COLUMN "ProductionOrderNumber" TYPE text USING "ProductionOrderNumber"::text;
                """);

            migrationBuilder.Sql("""DROP SEQUENCE IF EXISTS "ProductionOrderNumberSequence";""");
        }
    }
}
