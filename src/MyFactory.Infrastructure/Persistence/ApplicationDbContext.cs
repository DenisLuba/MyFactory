using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Entities.Security;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Orders;
using MyFactory.Domain.Entities.Organization;
using MyFactory.Domain.Entities.Parties;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Entities.Products;

namespace MyFactory.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<RoleEntity> Roles { get; set; } = default!;
    public DbSet<UserEntity> Users { get; set; } = default!;
    public DbSet<TokenEntity> Tokens { get; set; } = default!;

    public DbSet<ExpenseEntity> Expenses { get; set; } = default!;
    public DbSet<CashAdvanceEntity> CashAdvances { get; set; } = default!;
    public DbSet<CashAdvanceExpenseEntity> CashAdvanceExpenses { get; set; } = default!;
    public DbSet<CashAdvanceReturnEntity> CashAdvanceReturns { get; set; } = default!;
    public DbSet<MonthlyFinancialReportEntity> MonthlyFinancialReports { get; set; } = default!;

    public DbSet<MaterialEntity> Materials { get; set; } = default!;
    public DbSet<MaterialTypeEntity> MaterialTypes { get; set; } = default!;
    public DbSet<UnitEntity> Units { get; set; } = default!;
    public DbSet<WarehouseMaterialEntity> WarehouseMaterials { get; set; } = default!;
    public DbSet<WarehouseEntity> Warehouses { get; set; } = default!;
    public DbSet<MaterialPurchaseOrderEntity> MaterialPurchaseOrders { get; set; } = default!;
    public DbSet<MaterialPurchaseOrderItemEntity> MaterialPurchaseOrderItems { get; set; } = default!;
    public DbSet<SupplierEntity> Suppliers { get; set; } = default!;

    public DbSet<InventoryMovementEntity> InventoryMovements { get; set; } = default!;
    public DbSet<InventoryMovementItemEntity> InventoryMovementItems { get; set; } = default!;

    public DbSet<ProductEntity> Products { get; set; } = default!;
    public DbSet<ProductMaterialEntity> ProductMaterials { get; set; } = default!;
    public DbSet<ProductDepartmentCostEntity> ProductDepartmentCosts { get; set; } = default!;
    public DbSet<ProductImageEntity> ProductImages { get; set; } = default!;

    public DbSet<FinishedGoodsEntity> FinishedGoods { get; set; } = default!;
    public DbSet<FinishedGoodsStockEntity> FinishedGoodsStocks { get; set; } = default!;
    public DbSet<FinishedGoodsMovementEntity> FinishedGoodsMovements { get; set; } = default!;
    public DbSet<FinishedGoodsMovementItemEntity> FinishedGoodsMovementItems { get; set; } = default!;

    public DbSet<SalesOrderEntity> SalesOrders { get; set; } = default!;
    public DbSet<SalesOrderItemEntity> SalesOrderItems { get; set; } = default!;
    public DbSet<CustomerEntity> Customers { get; set; } = default!;

    public DbSet<ProductionOrderEntity> ProductionOrders { get; set; } = default!;

    public DbSet<ContactEntity> Contacts { get; set; } = default!;
    public DbSet<ContactLinkEntity> ContactLinks { get; set; } = default!;

    public DbSet<CuttingOperationEntity> CuttingOperations { get; set; } = default!;
    public DbSet<SewingOperationEntity> SewingOperations { get; set; } = default!;
    public DbSet<PackagingOperationEntity> PackagingOperations { get; set; } = default!;
    public DbSet<EmployeeEntity> Employees { get; set; } = default!;

    public DbSet<PositionEntity> Positions { get; set; } = default!;
    public DbSet<DepartmentEntity> Departments { get; set; } = default!;
    public DbSet<DepartmentPositionEntity> DepartmentPositions { get; set; } = default!;
    public DbSet<TimesheetEntity> Timesheets { get; set; } = default!;

    public DbSet<PayrollAccrualEntity> PayrollAccruals { get; set; } = default!;
    public DbSet<PayrollPaymentEntity> PayrollPayments { get; set; } = default!;
    public DbSet<PayrollRuleEntity> PayrollRules { get; set; } = default!;
    public DbSet<ExpenseTypeEntity> ExpenseTypes { get; set; } = default!;

    public DbSet<ShipmentEntity> Shipments { get; set; } = default!;
    public DbSet<ShipmentItemEntity> ShipmentItems { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}

