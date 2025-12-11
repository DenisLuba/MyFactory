using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Entities.Files;
using MyFactory.Domain.Entities.FinishedGoods;
using MyFactory.Domain.Entities.Identity;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Operations;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Entities.Sales;
using MyFactory.Domain.Entities.Shifts;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.Entities.Warehousing;
using MyFactory.Domain.Entities.Workshops;
using MyFactory.Infrastructure.Persistence.Auditing;
using MyFactory.Domain.Entities.Reports;

namespace MyFactory.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<MaterialType> MaterialTypes => Set<MaterialType>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<MaterialPriceHistory> MaterialPriceHistoryEntries => Set<MaterialPriceHistory>();
    public DbSet<Operation> Operations => Set<Operation>();
    public DbSet<Workshop> Workshops => Set<Workshop>();
    public DbSet<WorkshopExpenseHistory> WorkshopExpenseHistoryEntries => Set<WorkshopExpenseHistory>();
    public DbSet<Specification> Specifications => Set<Specification>();
    public DbSet<SpecificationBomItem> SpecificationBomItems => Set<SpecificationBomItem>();
    public DbSet<SpecificationOperation> SpecificationOperations => Set<SpecificationOperation>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<InventoryReceipt> InventoryReceipts => Set<InventoryReceipt>();
    public DbSet<InventoryReceiptItem> InventoryReceiptItems => Set<InventoryReceiptItem>();
    public DbSet<FinishedGoodsInventory> FinishedGoodsInventories => Set<FinishedGoodsInventory>();
    public DbSet<FinishedGoodsMovement> FinishedGoodsMovements => Set<FinishedGoodsMovement>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Shipment> Shipments => Set<Shipment>();
    public DbSet<ShipmentItem> ShipmentItems => Set<ShipmentItem>();
    public DbSet<CustomerReturn> CustomerReturns => Set<CustomerReturn>();
    public DbSet<CustomerReturnItem> CustomerReturnItems => Set<CustomerReturnItem>();
    public DbSet<PurchaseRequest> PurchaseRequests => Set<PurchaseRequest>();
    public DbSet<PurchaseRequestItem> PurchaseRequestItems => Set<PurchaseRequestItem>();
    public DbSet<ProductionOrder> ProductionOrders => Set<ProductionOrder>();
    public DbSet<ProductionOrderAllocation> ProductionOrderAllocations => Set<ProductionOrderAllocation>();
    public DbSet<ProductionStage> ProductionStages => Set<ProductionStage>();
    public DbSet<WorkerAssignment> WorkerAssignments => Set<WorkerAssignment>();
    public DbSet<ShiftPlan> ShiftPlans => Set<ShiftPlan>();
    public DbSet<ShiftResult> ShiftResults => Set<ShiftResult>();
    public DbSet<TimesheetEntry> TimesheetEntries => Set<TimesheetEntry>();
    public DbSet<PayrollEntry> PayrollEntries => Set<PayrollEntry>();
    public DbSet<ExpenseType> ExpenseTypes => Set<ExpenseType>();
    public DbSet<OverheadMonthly> OverheadMonthlyEntries => Set<OverheadMonthly>();
    public DbSet<RevenueReport> RevenueReports => Set<RevenueReport>();
    public DbSet<ProductionCostFact> ProductionCostFacts => Set<ProductionCostFact>();
    public DbSet<MonthlyProfit> MonthlyProfits => Set<MonthlyProfit>();
    public DbSet<Advance> Advances => Set<Advance>();
    public DbSet<AdvanceReport> AdvanceReports => Set<AdvanceReport>();
    public DbSet<FileResource> FileResources => Set<FileResource>();
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

    private void ApplyAuditInformation()
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                if (AuditMetadata.ShouldSetCreatedAt(entry.Entity) && entry.Entity.CreatedAt == default)
                {
                    entry.Entity.CreatedAt = utcNow;
                }

                if (AuditMetadata.ShouldSetUpdatedAt(entry.Entity) && !entry.Property(nameof(BaseEntity.UpdatedAt)).IsModified)
                {
                    entry.Entity.UpdatedAt = entry.Entity.UpdatedAt ?? entry.Entity.CreatedAt;
                }

                entry.Entity.IsDeleted = false;
            }

            if (entry.State == EntityState.Modified)
            {
                ApplyUpdatedAt(entry, utcNow);
            }

            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                ApplyUpdatedAt(entry, utcNow);
            }
        }
    }

    private static void ApplyUpdatedAt(EntityEntry<BaseEntity> entry, DateTime utcNow)
    {
        var updatedAtProperty = entry.Property(nameof(BaseEntity.UpdatedAt));
        if (updatedAtProperty.IsModified)
        {
            return;
        }

        if (AuditMetadata.ShouldSetUpdatedAt(entry.Entity))
        {
            entry.Entity.UpdatedAt = utcNow;
        }
    }

    private static void ApplySoftDeleteFilters(ModelBuilder modelBuilder)
    {
        var methodInfo = typeof(ApplicationDbContext)
            .GetMethod(nameof(ConfigureSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            var genericMethod = methodInfo!.MakeGenericMethod(entityType.ClrType);
            genericMethod.Invoke(null, new object[] { modelBuilder });
        }
    }

    private static void ConfigureSoftDeleteFilter<TEntity>(ModelBuilder modelBuilder)
        where TEntity : BaseEntity
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(entity => !entity.IsDeleted);
    }
}

