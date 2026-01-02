using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
using MyFactory.Infrastructure.Persistence.Auditing;

namespace MyFactory.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<RoleEntity> Roles => throw new NotImplementedException();
    public DbSet<UserEntity> Users => throw new NotImplementedException();
    
    public DbSet<ExpenseEntity> Expenses => throw new NotImplementedException();
    public DbSet<CashAdvanceEntity> CashAdvances => throw new NotImplementedException();
    public DbSet<CashAdvanceExpenseEntity> CashAdvanceExpenses => throw new NotImplementedException();
    public DbSet<CashAdvanceReturnEntity> CashAdvanceReturns => throw new NotImplementedException();
    public DbSet<MonthlyFinancialReportEntity> MonthlyFinancialReports => throw new NotImplementedException();

    public DbSet<MaterialEntity> Materials => throw new NotImplementedException();

    public DbSet<MaterialTypeEntity> MaterialTypes => throw new NotImplementedException();

    public DbSet<UnitEntity> Units => throw new NotImplementedException();

    public DbSet<WarehouseMaterialEntity> WarehouseMaterials => throw new NotImplementedException();

    public DbSet<WarehouseEntity> Warehouses => throw new NotImplementedException();

    public DbSet<MaterialPurchaseOrderEntity> MaterialPurchaseOrders => throw new NotImplementedException();

    public DbSet<MaterialPurchaseOrderItemEntity> MaterialPurchaseOrderItems => throw new NotImplementedException();

    public DbSet<SupplierEntity> Suppliers => throw new NotImplementedException();

    public DbSet<InventoryMovementEntity> InventoryMovements => throw new NotImplementedException();

    public DbSet<InventoryMovementItemEntity> InventoryMovementItems => throw new NotImplementedException();

    public DbSet<ProductEntity> Products => throw new NotImplementedException();

    public DbSet<ProductMaterialEntity> ProductMaterials => throw new NotImplementedException();

    public DbSet<ProductDepartmentCostEntity> ProductDepartmentCosts => throw new NotImplementedException();

    public DbSet<FinishedGoodsEntity> FinishedGoods => throw new NotImplementedException();

    public DbSet<FinishedGoodsStockEntity> FinishedGoodsStocks => throw new NotImplementedException();

    public DbSet<FinishedGoodsMovementEntity> FinishedGoodsMovements => throw new NotImplementedException();

    public DbSet<FinishedGoodsMovementItemEntity> FinishedGoodsMovementItems => throw new NotImplementedException();

    public DbSet<SalesOrderEntity> SalesOrders => throw new NotImplementedException();

    public DbSet<SalesOrderItemEntity> SalesOrderItems => throw new NotImplementedException();

    public DbSet<CustomerEntity> Customers => throw new NotImplementedException();

    public DbSet<ProductionOrderEntity> ProductionOrders => throw new NotImplementedException();

    public DbSet<ContactEntity> Contacts => throw new NotImplementedException();

    public DbSet<ContactLinkEntity> ContactLinks => throw new NotImplementedException();

    public DbSet<CuttingOperationEntity> CuttingOperations => throw new NotImplementedException();

    public DbSet<SewingOperationEntity> SewingOperations => throw new NotImplementedException();

    public DbSet<PackagingOperationEntity> PackagingOperations => throw new NotImplementedException();

    public DbSet<EmployeeEntity> Employees => throw new NotImplementedException();

    public DbSet<PositionEntity> Positions => throw new NotImplementedException();

    public DbSet<DepartmentEntity> Departments => throw new NotImplementedException();

    public DbSet<TimesheetEntity> Timesheets => throw new NotImplementedException();

    public DbSet<PayrollAccrualEntity> PayrollAccruals => throw new NotImplementedException();

    public DbSet<PayrollPaymentEntity> PayrollPayments => throw new NotImplementedException();

    public DbSet<PayrollRuleEntity> PayrollRules => throw new NotImplementedException();

    public DbSet<ExpenseTypeEntity> ExpenseTypes => throw new NotImplementedException();

    public DbSet<ShipmentEntity> Shipments => throw new NotImplementedException();
    public DbSet<ShipmentItemEntity> ShipmentItems => throw new NotImplementedException();

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyAuditInformation();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplyAuditInformation();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return SaveChangesAsync(true, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        ApplySoftDeleteFilters(modelBuilder);

    }

    //private void ApplyAuditInformation()
    //{
    //    var utcNow = DateTime.UtcNow;

    //    foreach (var entry in ChangeTracker.Entries<BaseEntity>())
    //    {
    //        if (entry.State == EntityState.Added)
    //        {
    //            if (AuditMetadata.ShouldSetCreatedAt(entry.Entity) && entry.Entity.CreatedAt == default)
    //            {
    //                entry.Entity.CreatedAt = utcNow;
    //            }

    //            if (AuditMetadata.ShouldSetUpdatedAt(entry.Entity) && !entry.Property(nameof(BaseEntity.UpdatedAt)).IsModified)
    //            {
    //                entry.Entity.UpdatedAt = entry.Entity.UpdatedAt ?? entry.Entity.CreatedAt;
    //            }

    //            entry.Entity.IsDeleted = false;
    //        }

    //        if (entry.State == EntityState.Modified)
    //        {
    //            ApplyUpdatedAt(entry, utcNow);
    //        }

    //        if (entry.State == EntityState.Deleted)
    //        {
    //            entry.State = EntityState.Modified;
    //            entry.Entity.IsDeleted = true;
    //            ApplyUpdatedAt(entry, utcNow);
    //        }
    //    }
    //}

    //private static void ApplyUpdatedAt(EntityEntry<BaseEntity> entry, DateTime utcNow)
    //{
    //    var updatedAtProperty = entry.Property(nameof(BaseEntity.UpdatedAt));
    //    if (updatedAtProperty.IsModified)
    //    {
    //        return;
    //    }

    //    if (AuditMetadata.ShouldSetUpdatedAt(entry.Entity))
    //    {
    //        entry.Entity.UpdatedAt = utcNow;
    //    }
    //}

    //private static void ApplySoftDeleteFilters(ModelBuilder modelBuilder)
    //{
    //    var methodInfo = typeof(ApplicationDbContext)
    //        .GetMethod(nameof(ConfigureSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static);

    //    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
    //    {
    //        if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
    //        {
    //            continue;
    //        }

    //        var genericMethod = methodInfo!.MakeGenericMethod(entityType.ClrType);
    //        genericMethod.Invoke(null, new object[] { modelBuilder });
    //    }
    //}

    //private static void ConfigureSoftDeleteFilter<TEntity>(ModelBuilder modelBuilder)
    //    where TEntity : BaseEntity
    //{
    //    modelBuilder.Entity<TEntity>().HasQueryFilter(entity => !entity.IsDeleted);
    //}
}

