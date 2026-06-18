using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFactory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CASH_ADVANCE_EXPENSES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CashAdvanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpenseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CASH_ADVANCE_EXPENSES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CASH_ADVANCE_RETURNS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CashAdvanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReturnDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CASH_ADVANCE_RETURNS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CONTACTS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactType = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTACTS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CUSTOMERS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUSTOMERS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DEPARTMENTS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEPARTMENTS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EXPENSE_TYPES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXPENSE_TYPES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EXPENSES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpenseTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpenseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXPENSES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MATERIAL_TYPES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MATERIAL_TYPES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MONTHLY_FINANCIAL_REPORTS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportYear = table.Column<int>(type: "integer", nullable: false),
                    ReportMonth = table.Column<int>(type: "integer", nullable: false),
                    TotalRevenue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PayrollExpenses = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MaterialExpenses = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    OtherExpenses = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CalculatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MONTHLY_FINANCIAL_REPORTS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PAYROLL_RULES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EffectiveFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    PremiumPercent = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAYROLL_RULES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ROLES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SUPPLIERS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUPPLIERS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UNITS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UNITS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WAREHOUSES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WAREHOUSES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SALES_ORDERS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderNumber = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RequiredByDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALES_ORDERS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SALES_ORDERS_CUSTOMERS_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "CUSTOMERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "POSITIONS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    BaseNormPerHour = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    BaseRatePerNormHour = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    DefaultPremiumPercent = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CanCut = table.Column<bool>(type: "boolean", nullable: false),
                    CanSew = table.Column<bool>(type: "boolean", nullable: false),
                    CanPackage = table.Column<bool>(type: "boolean", nullable: false),
                    CanHandleMaterials = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POSITIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POSITIONS_DEPARTMENTS_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DEPARTMENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Sku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    PlanPerHour = table.Column<int>(type: "integer", nullable: true),
                    PayrollRuleId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRODUCTS_PAYROLL_RULES_PayrollRuleId",
                        column: x => x.PayrollRuleId,
                        principalTable: "PAYROLL_RULES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MATERIAL_PURCHASE_ORDERS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MATERIAL_PURCHASE_ORDERS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MATERIAL_PURCHASE_ORDERS_SUPPLIERS_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "SUPPLIERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MATERIALS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    MaterialTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    UnitId = table.Column<Guid>(type: "uuid", nullable: false),
                    Color = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MATERIALS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MATERIALS_MATERIAL_TYPES_MaterialTypeId",
                        column: x => x.MaterialTypeId,
                        principalTable: "MATERIAL_TYPES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MATERIALS_UNITS_UnitId",
                        column: x => x.UnitId,
                        principalTable: "UNITS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FINISHED_GOODS_MOVEMENTS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FromWarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToWarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    MovementDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINISHED_GOODS_MOVEMENTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FINISHED_GOODS_MOVEMENTS_USERS_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FINISHED_GOODS_MOVEMENTS_WAREHOUSES_FromWarehouseId",
                        column: x => x.FromWarehouseId,
                        principalTable: "WAREHOUSES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FINISHED_GOODS_MOVEMENTS_WAREHOUSES_ToWarehouseId",
                        column: x => x.ToWarehouseId,
                        principalTable: "WAREHOUSES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SALES_ORDER_ITEMS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SalesOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    QtyOrdered = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    QtyAllocated = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    QtyShipped = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALES_ORDER_ITEMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SALES_ORDER_ITEMS_SALES_ORDERS_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalTable: "SALES_ORDERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SHIPMENTS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SalesOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShipmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHIPMENTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SHIPMENTS_CUSTOMERS_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "CUSTOMERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SHIPMENTS_SALES_ORDERS_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalTable: "SALES_ORDERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PositionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Grade = table.Column<int>(type: "integer", nullable: false),
                    RatePerNormHour = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PremiumPercent = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    HiredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FiredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EMPLOYEES_POSITIONS_PositionId",
                        column: x => x.PositionId,
                        principalTable: "POSITIONS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FINISHED_GOODS_STOCK",
                columns: table => new
                {
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Qty = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINISHED_GOODS_STOCK", x => new { x.WarehouseId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_FINISHED_GOODS_STOCK_PRODUCTS_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PRODUCTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FINISHED_GOODS_STOCK_WAREHOUSES_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "WAREHOUSES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCT_DEPARTMENT_COSTS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpensesPerUnit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CutCostPerUnit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SewingCostPerUnit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PackCostPerUnit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT_DEPARTMENT_COSTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRODUCT_DEPARTMENT_COSTS_DEPARTMENTS_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DEPARTMENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PRODUCT_DEPARTMENT_COSTS_PRODUCTS_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PRODUCTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCT_IMAGES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT_IMAGES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRODUCT_IMAGES_PRODUCTS_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PRODUCTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WAREHOUSE_PRODUCTS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Qty = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WAREHOUSE_PRODUCTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WAREHOUSE_PRODUCTS_PRODUCTS_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PRODUCTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WAREHOUSE_PRODUCTS_WAREHOUSES_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "WAREHOUSES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MATERIAL_PURCHASE_ITEMS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    Qty = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MaterialName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UnitCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MATERIAL_PURCHASE_ITEMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MATERIAL_PURCHASE_ITEMS_MATERIALS_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "MATERIALS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MATERIAL_PURCHASE_ITEMS_MATERIAL_PURCHASE_ORDERS_PurchaseOr~",
                        column: x => x.PurchaseOrderId,
                        principalTable: "MATERIAL_PURCHASE_ORDERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MATERIAL_SUPPLIERS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    MinOrderQty = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MATERIAL_SUPPLIERS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MATERIAL_SUPPLIERS_MATERIALS_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "MATERIALS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MATERIAL_SUPPLIERS_SUPPLIERS_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "SUPPLIERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCT_MATERIALS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    QtyPerUnit = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT_MATERIALS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRODUCT_MATERIALS_MATERIALS_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "MATERIALS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PRODUCT_MATERIALS_PRODUCTS_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PRODUCTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WAREHOUSE_MATERIALS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    Qty = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WAREHOUSE_MATERIALS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WAREHOUSE_MATERIALS_MATERIALS_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "MATERIALS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WAREHOUSE_MATERIALS_WAREHOUSES_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "WAREHOUSES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FINISHED_GOODS_MOVEMENT_ITEMS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MovementId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Qty = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINISHED_GOODS_MOVEMENT_ITEMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FINISHED_GOODS_MOVEMENT_ITEMS_FINISHED_GOODS_MOVEMENTS_Move~",
                        column: x => x.MovementId,
                        principalTable: "FINISHED_GOODS_MOVEMENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FINISHED_GOODS_MOVEMENT_ITEMS_PRODUCTS_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PRODUCTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTION_ORDERS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductionOrderNumber = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SalesOrderItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    QtyPlanned = table.Column<int>(type: "integer", nullable: false),
                    QtyFinished = table.Column<int>(type: "integer", nullable: false),
                    QtyCut = table.Column<int>(type: "integer", nullable: false),
                    QtySewn = table.Column<int>(type: "integer", nullable: false),
                    QtyPacked = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTION_ORDERS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRODUCTION_ORDERS_DEPARTMENTS_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DEPARTMENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PRODUCTION_ORDERS_SALES_ORDER_ITEMS_SalesOrderItemId",
                        column: x => x.SalesOrderItemId,
                        principalTable: "SALES_ORDER_ITEMS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SHIPMENT_ITEMS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ShipmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    SalesOrderItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Qty = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHIPMENT_ITEMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SHIPMENT_ITEMS_PRODUCTS_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PRODUCTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SHIPMENT_ITEMS_SALES_ORDER_ITEMS_SalesOrderItemId",
                        column: x => x.SalesOrderItemId,
                        principalTable: "SALES_ORDER_ITEMS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SHIPMENT_ITEMS_SHIPMENTS_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "SHIPMENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SHIPMENT_ITEMS_WAREHOUSES_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "WAREHOUSES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SHIPMENT_RETURNS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ShipmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    SalesOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHIPMENT_RETURNS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SHIPMENT_RETURNS_CUSTOMERS_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "CUSTOMERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SHIPMENT_RETURNS_SALES_ORDERS_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalTable: "SALES_ORDERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SHIPMENT_RETURNS_SHIPMENTS_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "SHIPMENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CASH_ADVANCES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ClosedAt = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CASH_ADVANCES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CASH_ADVANCES_EMPLOYEES_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "EMPLOYEES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CONTACT_LINKS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerType = table.Column<int>(type: "integer", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTACT_LINKS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CONTACT_LINKS_CONTACTS_ContactId",
                        column: x => x.ContactId,
                        principalTable: "CONTACTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CONTACT_LINKS_CUSTOMERS_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "CUSTOMERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CONTACT_LINKS_EMPLOYEES_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "EMPLOYEES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CONTACT_LINKS_USERS_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PAYROLL_ACCRUALS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccrualDate = table.Column<DateOnly>(type: "date", nullable: false),
                    HoursWorked = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    QtyPlanned = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    QtyProduced = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    QtyExtra = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    BaseAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PremiumAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AdjustmentReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAYROLL_ACCRUALS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PAYROLL_ACCRUALS_EMPLOYEES_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "EMPLOYEES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PAYROLL_PAYMENTS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAYROLL_PAYMENTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PAYROLL_PAYMENTS_EMPLOYEES_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "EMPLOYEES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TIMESHEETS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkDate = table.Column<DateOnly>(type: "date", nullable: false),
                    HoursWorked = table.Column<decimal>(type: "numeric", nullable: false),
                    Comment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIMESHEETS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TIMESHEETS_DEPARTMENTS_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DEPARTMENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TIMESHEETS_EMPLOYEES_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "EMPLOYEES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CUTTING_OPERATIONS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductionOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    QtyPlanned = table.Column<int>(type: "integer", nullable: false),
                    QtyCut = table.Column<int>(type: "integer", nullable: false),
                    OperationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUTTING_OPERATIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CUTTING_OPERATIONS_EMPLOYEES_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "EMPLOYEES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CUTTING_OPERATIONS_PRODUCTION_ORDERS_ProductionOrderId",
                        column: x => x.ProductionOrderId,
                        principalTable: "PRODUCTION_ORDERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FINISHED_GOODS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductionOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Qty = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINISHED_GOODS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FINISHED_GOODS_PRODUCTION_ORDERS_ProductionOrderId",
                        column: x => x.ProductionOrderId,
                        principalTable: "PRODUCTION_ORDERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FINISHED_GOODS_PRODUCTS_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PRODUCTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FINISHED_GOODS_WAREHOUSES_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "WAREHOUSES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "INVENTORY_MOVEMENTS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MovementType = table.Column<int>(type: "integer", nullable: false),
                    FromWarehouseId = table.Column<Guid>(type: "uuid", nullable: true),
                    ToWarehouseId = table.Column<Guid>(type: "uuid", nullable: true),
                    ToDepartmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProductionOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INVENTORY_MOVEMENTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_INVENTORY_MOVEMENTS_DEPARTMENTS_ToDepartmentId",
                        column: x => x.ToDepartmentId,
                        principalTable: "DEPARTMENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_INVENTORY_MOVEMENTS_PRODUCTION_ORDERS_ProductionOrderId",
                        column: x => x.ProductionOrderId,
                        principalTable: "PRODUCTION_ORDERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_INVENTORY_MOVEMENTS_USERS_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_INVENTORY_MOVEMENTS_WAREHOUSES_FromWarehouseId",
                        column: x => x.FromWarehouseId,
                        principalTable: "WAREHOUSES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_INVENTORY_MOVEMENTS_WAREHOUSES_ToWarehouseId",
                        column: x => x.ToWarehouseId,
                        principalTable: "WAREHOUSES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PACKAGING_OPERATIONS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductionOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    QtyPlanned = table.Column<int>(type: "integer", nullable: false),
                    QtyPacked = table.Column<int>(type: "integer", nullable: false),
                    OperationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PACKAGING_OPERATIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PACKAGING_OPERATIONS_EMPLOYEES_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "EMPLOYEES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PACKAGING_OPERATIONS_PRODUCTION_ORDERS_ProductionOrderId",
                        column: x => x.ProductionOrderId,
                        principalTable: "PRODUCTION_ORDERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SEWING_OPERATIONS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductionOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    QtyPlanned = table.Column<int>(type: "integer", nullable: false),
                    QtySewn = table.Column<int>(type: "integer", nullable: false),
                    HoursWorked = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    OperationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEWING_OPERATIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SEWING_OPERATIONS_EMPLOYEES_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "EMPLOYEES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SEWING_OPERATIONS_PRODUCTION_ORDERS_ProductionOrderId",
                        column: x => x.ProductionOrderId,
                        principalTable: "PRODUCTION_ORDERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SHIPMENT_RETURN_ITEMS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ShipmentReturnId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShipmentItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    SalesOrderItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Qty = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Condition = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHIPMENT_RETURN_ITEMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SHIPMENT_RETURN_ITEMS_PRODUCTS_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PRODUCTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SHIPMENT_RETURN_ITEMS_SALES_ORDER_ITEMS_SalesOrderItemId",
                        column: x => x.SalesOrderItemId,
                        principalTable: "SALES_ORDER_ITEMS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SHIPMENT_RETURN_ITEMS_SHIPMENT_ITEMS_ShipmentItemId",
                        column: x => x.ShipmentItemId,
                        principalTable: "SHIPMENT_ITEMS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SHIPMENT_RETURN_ITEMS_SHIPMENT_RETURNS_ShipmentReturnId",
                        column: x => x.ShipmentReturnId,
                        principalTable: "SHIPMENT_RETURNS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SHIPMENT_RETURN_ITEMS_WAREHOUSES_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "WAREHOUSES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "INVENTORY_MOVEMENT_ITEMS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MovementId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    Qty = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INVENTORY_MOVEMENT_ITEMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_INVENTORY_MOVEMENT_ITEMS_INVENTORY_MOVEMENTS_MovementId",
                        column: x => x.MovementId,
                        principalTable: "INVENTORY_MOVEMENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_INVENTORY_MOVEMENT_ITEMS_MATERIALS_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "MATERIALS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CASH_ADVANCE_EXPENSES_CashAdvanceId_ExpenseDate",
                table: "CASH_ADVANCE_EXPENSES",
                columns: new[] { "CashAdvanceId", "ExpenseDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CASH_ADVANCE_RETURNS_CashAdvanceId_ReturnDate",
                table: "CASH_ADVANCE_RETURNS",
                columns: new[] { "CashAdvanceId", "ReturnDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CASH_ADVANCES_EmployeeId_IssueDate",
                table: "CASH_ADVANCES",
                columns: new[] { "EmployeeId", "IssueDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CONTACT_LINKS_ContactId",
                table: "CONTACT_LINKS",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_CONTACT_LINKS_OwnerId",
                table: "CONTACT_LINKS",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CUSTOMERS_Name",
                table: "CUSTOMERS",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CUTTING_OPERATIONS_EmployeeId",
                table: "CUTTING_OPERATIONS",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CUTTING_OPERATIONS_ProductionOrderId",
                table: "CUTTING_OPERATIONS",
                column: "ProductionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DEPARTMENTS_Name",
                table: "DEPARTMENTS",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EMPLOYEES_PositionId",
                table: "EMPLOYEES",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_EXPENSE_TYPES_Name",
                table: "EXPENSE_TYPES",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EXPENSES_ExpenseDate",
                table: "EXPENSES",
                column: "ExpenseDate");

            migrationBuilder.CreateIndex(
                name: "IX_FINISHED_GOODS_ProductId_WarehouseId_ProductionOrderId",
                table: "FINISHED_GOODS",
                columns: new[] { "ProductId", "WarehouseId", "ProductionOrderId" });

            migrationBuilder.CreateIndex(
                name: "IX_FINISHED_GOODS_ProductionOrderId",
                table: "FINISHED_GOODS",
                column: "ProductionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_FINISHED_GOODS_WarehouseId",
                table: "FINISHED_GOODS",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_FINISHED_GOODS_MOVEMENT_ITEMS_MovementId",
                table: "FINISHED_GOODS_MOVEMENT_ITEMS",
                column: "MovementId");

            migrationBuilder.CreateIndex(
                name: "IX_FINISHED_GOODS_MOVEMENT_ITEMS_ProductId",
                table: "FINISHED_GOODS_MOVEMENT_ITEMS",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FINISHED_GOODS_MOVEMENTS_CreatedByUserId",
                table: "FINISHED_GOODS_MOVEMENTS",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FINISHED_GOODS_MOVEMENTS_FromWarehouseId_ToWarehouseId_Move~",
                table: "FINISHED_GOODS_MOVEMENTS",
                columns: new[] { "FromWarehouseId", "ToWarehouseId", "MovementDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FINISHED_GOODS_MOVEMENTS_ToWarehouseId",
                table: "FINISHED_GOODS_MOVEMENTS",
                column: "ToWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_FINISHED_GOODS_STOCK_ProductId",
                table: "FINISHED_GOODS_STOCK",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_INVENTORY_MOVEMENT_ITEMS_MaterialId",
                table: "INVENTORY_MOVEMENT_ITEMS",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_INVENTORY_MOVEMENT_ITEMS_MovementId",
                table: "INVENTORY_MOVEMENT_ITEMS",
                column: "MovementId");

            migrationBuilder.CreateIndex(
                name: "IX_INVENTORY_MOVEMENTS_CreatedBy",
                table: "INVENTORY_MOVEMENTS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_INVENTORY_MOVEMENTS_CreatedByUserId",
                table: "INVENTORY_MOVEMENTS",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_INVENTORY_MOVEMENTS_FromWarehouseId",
                table: "INVENTORY_MOVEMENTS",
                column: "FromWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_INVENTORY_MOVEMENTS_ProductionOrderId",
                table: "INVENTORY_MOVEMENTS",
                column: "ProductionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_INVENTORY_MOVEMENTS_ToDepartmentId",
                table: "INVENTORY_MOVEMENTS",
                column: "ToDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_INVENTORY_MOVEMENTS_ToWarehouseId",
                table: "INVENTORY_MOVEMENTS",
                column: "ToWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_MATERIAL_PURCHASE_ITEMS_MaterialId",
                table: "MATERIAL_PURCHASE_ITEMS",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MATERIAL_PURCHASE_ITEMS_PurchaseOrderId",
                table: "MATERIAL_PURCHASE_ITEMS",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_MATERIAL_PURCHASE_ORDERS_SupplierId_OrderDate",
                table: "MATERIAL_PURCHASE_ORDERS",
                columns: new[] { "SupplierId", "OrderDate" });

            migrationBuilder.CreateIndex(
                name: "IX_MATERIAL_SUPPLIERS_MaterialId_SupplierId",
                table: "MATERIAL_SUPPLIERS",
                columns: new[] { "MaterialId", "SupplierId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MATERIAL_SUPPLIERS_SupplierId",
                table: "MATERIAL_SUPPLIERS",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_MATERIAL_TYPES_Name",
                table: "MATERIAL_TYPES",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MATERIALS_MaterialTypeId",
                table: "MATERIALS",
                column: "MaterialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MATERIALS_Name",
                table: "MATERIALS",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MATERIALS_UnitId",
                table: "MATERIALS",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MONTHLY_FINANCIAL_REPORTS_ReportYear_ReportMonth",
                table: "MONTHLY_FINANCIAL_REPORTS",
                columns: new[] { "ReportYear", "ReportMonth" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PACKAGING_OPERATIONS_EmployeeId",
                table: "PACKAGING_OPERATIONS",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PACKAGING_OPERATIONS_ProductionOrderId",
                table: "PACKAGING_OPERATIONS",
                column: "ProductionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PAYROLL_ACCRUALS_EmployeeId_AccrualDate",
                table: "PAYROLL_ACCRUALS",
                columns: new[] { "EmployeeId", "AccrualDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PAYROLL_PAYMENTS_EmployeeId_PaymentDate",
                table: "PAYROLL_PAYMENTS",
                columns: new[] { "EmployeeId", "PaymentDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PAYROLL_RULES_EffectiveFrom",
                table: "PAYROLL_RULES",
                column: "EffectiveFrom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_POSITIONS_Code",
                table: "POSITIONS",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_POSITIONS_DepartmentId",
                table: "POSITIONS",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_POSITIONS_Name",
                table: "POSITIONS",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_DEPARTMENT_COSTS_DepartmentId",
                table: "PRODUCT_DEPARTMENT_COSTS",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_DEPARTMENT_COSTS_ProductId_DepartmentId",
                table: "PRODUCT_DEPARTMENT_COSTS",
                columns: new[] { "ProductId", "DepartmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_IMAGES_ProductId",
                table: "PRODUCT_IMAGES",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_MATERIALS_MaterialId",
                table: "PRODUCT_MATERIALS",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_MATERIALS_ProductId_MaterialId",
                table: "PRODUCT_MATERIALS",
                columns: new[] { "ProductId", "MaterialId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTION_ORDERS_DepartmentId",
                table: "PRODUCTION_ORDERS",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTION_ORDERS_ProductionOrderNumber",
                table: "PRODUCTION_ORDERS",
                column: "ProductionOrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTION_ORDERS_SalesOrderItemId",
                table: "PRODUCTION_ORDERS",
                column: "SalesOrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTS_PayrollRuleId",
                table: "PRODUCTS",
                column: "PayrollRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTS_Sku",
                table: "PRODUCTS",
                column: "Sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ROLES_Name",
                table: "ROLES",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SALES_ORDER_ITEMS_SalesOrderId",
                table: "SALES_ORDER_ITEMS",
                column: "SalesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SALES_ORDERS_CustomerId",
                table: "SALES_ORDERS",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SALES_ORDERS_OrderNumber",
                table: "SALES_ORDERS",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SEWING_OPERATIONS_EmployeeId",
                table: "SEWING_OPERATIONS",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SEWING_OPERATIONS_ProductionOrderId",
                table: "SEWING_OPERATIONS",
                column: "ProductionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SHIPMENT_ITEMS_ProductId",
                table: "SHIPMENT_ITEMS",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SHIPMENT_ITEMS_SalesOrderItemId",
                table: "SHIPMENT_ITEMS",
                column: "SalesOrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SHIPMENT_ITEMS_ShipmentId",
                table: "SHIPMENT_ITEMS",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SHIPMENT_ITEMS_WarehouseId",
                table: "SHIPMENT_ITEMS",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_SHIPMENT_RETURN_ITEMS_ProductId",
                table: "SHIPMENT_RETURN_ITEMS",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SHIPMENT_RETURN_ITEMS_SalesOrderItemId",
                table: "SHIPMENT_RETURN_ITEMS",
                column: "SalesOrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SHIPMENT_RETURN_ITEMS_ShipmentItemId",
                table: "SHIPMENT_RETURN_ITEMS",
                column: "ShipmentItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SHIPMENT_RETURN_ITEMS_ShipmentReturnId",
                table: "SHIPMENT_RETURN_ITEMS",
                column: "ShipmentReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_SHIPMENT_RETURN_ITEMS_WarehouseId",
                table: "SHIPMENT_RETURN_ITEMS",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_SHIPMENT_RETURNS_CustomerId",
                table: "SHIPMENT_RETURNS",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SHIPMENT_RETURNS_SalesOrderId",
                table: "SHIPMENT_RETURNS",
                column: "SalesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SHIPMENT_RETURNS_ShipmentId",
                table: "SHIPMENT_RETURNS",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SHIPMENTS_CustomerId",
                table: "SHIPMENTS",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SHIPMENTS_SalesOrderId",
                table: "SHIPMENTS",
                column: "SalesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SUPPLIERS_Name",
                table: "SUPPLIERS",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TIMESHEETS_DepartmentId",
                table: "TIMESHEETS",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TIMESHEETS_EmployeeId_WorkDate",
                table: "TIMESHEETS",
                columns: new[] { "EmployeeId", "WorkDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UNITS_Code",
                table: "UNITS",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USERS_Username",
                table: "USERS",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WAREHOUSE_MATERIALS_MaterialId",
                table: "WAREHOUSE_MATERIALS",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_WAREHOUSE_MATERIALS_WarehouseId_MaterialId",
                table: "WAREHOUSE_MATERIALS",
                columns: new[] { "WarehouseId", "MaterialId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WAREHOUSE_PRODUCTS_ProductId",
                table: "WAREHOUSE_PRODUCTS",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WAREHOUSE_PRODUCTS_WarehouseId_ProductId",
                table: "WAREHOUSE_PRODUCTS",
                columns: new[] { "WarehouseId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WAREHOUSES_Name",
                table: "WAREHOUSES",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CASH_ADVANCE_EXPENSES");

            migrationBuilder.DropTable(
                name: "CASH_ADVANCE_RETURNS");

            migrationBuilder.DropTable(
                name: "CASH_ADVANCES");

            migrationBuilder.DropTable(
                name: "CONTACT_LINKS");

            migrationBuilder.DropTable(
                name: "CUTTING_OPERATIONS");

            migrationBuilder.DropTable(
                name: "EXPENSE_TYPES");

            migrationBuilder.DropTable(
                name: "EXPENSES");

            migrationBuilder.DropTable(
                name: "FINISHED_GOODS");

            migrationBuilder.DropTable(
                name: "FINISHED_GOODS_MOVEMENT_ITEMS");

            migrationBuilder.DropTable(
                name: "FINISHED_GOODS_STOCK");

            migrationBuilder.DropTable(
                name: "INVENTORY_MOVEMENT_ITEMS");

            migrationBuilder.DropTable(
                name: "MATERIAL_PURCHASE_ITEMS");

            migrationBuilder.DropTable(
                name: "MATERIAL_SUPPLIERS");

            migrationBuilder.DropTable(
                name: "MONTHLY_FINANCIAL_REPORTS");

            migrationBuilder.DropTable(
                name: "PACKAGING_OPERATIONS");

            migrationBuilder.DropTable(
                name: "PAYROLL_ACCRUALS");

            migrationBuilder.DropTable(
                name: "PAYROLL_PAYMENTS");

            migrationBuilder.DropTable(
                name: "PRODUCT_DEPARTMENT_COSTS");

            migrationBuilder.DropTable(
                name: "PRODUCT_IMAGES");

            migrationBuilder.DropTable(
                name: "PRODUCT_MATERIALS");

            migrationBuilder.DropTable(
                name: "ROLES");

            migrationBuilder.DropTable(
                name: "SEWING_OPERATIONS");

            migrationBuilder.DropTable(
                name: "SHIPMENT_RETURN_ITEMS");

            migrationBuilder.DropTable(
                name: "TIMESHEETS");

            migrationBuilder.DropTable(
                name: "WAREHOUSE_MATERIALS");

            migrationBuilder.DropTable(
                name: "WAREHOUSE_PRODUCTS");

            migrationBuilder.DropTable(
                name: "CONTACTS");

            migrationBuilder.DropTable(
                name: "FINISHED_GOODS_MOVEMENTS");

            migrationBuilder.DropTable(
                name: "INVENTORY_MOVEMENTS");

            migrationBuilder.DropTable(
                name: "MATERIAL_PURCHASE_ORDERS");

            migrationBuilder.DropTable(
                name: "SHIPMENT_ITEMS");

            migrationBuilder.DropTable(
                name: "SHIPMENT_RETURNS");

            migrationBuilder.DropTable(
                name: "EMPLOYEES");

            migrationBuilder.DropTable(
                name: "MATERIALS");

            migrationBuilder.DropTable(
                name: "PRODUCTION_ORDERS");

            migrationBuilder.DropTable(
                name: "USERS");

            migrationBuilder.DropTable(
                name: "SUPPLIERS");

            migrationBuilder.DropTable(
                name: "PRODUCTS");

            migrationBuilder.DropTable(
                name: "WAREHOUSES");

            migrationBuilder.DropTable(
                name: "SHIPMENTS");

            migrationBuilder.DropTable(
                name: "POSITIONS");

            migrationBuilder.DropTable(
                name: "MATERIAL_TYPES");

            migrationBuilder.DropTable(
                name: "UNITS");

            migrationBuilder.DropTable(
                name: "SALES_ORDER_ITEMS");

            migrationBuilder.DropTable(
                name: "PAYROLL_RULES");

            migrationBuilder.DropTable(
                name: "DEPARTMENTS");

            migrationBuilder.DropTable(
                name: "SALES_ORDERS");

            migrationBuilder.DropTable(
                name: "CUSTOMERS");
        }
    }
}
