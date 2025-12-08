using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
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
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => base.SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>(builder =>
        {
            builder.ToTable("Roles");
            builder.Property(role => role.Name)
                .IsRequired()
                .HasMaxLength(256);
            builder.Property(role => role.Description)
                .HasMaxLength(Role.DescriptionMaxLength);
            builder.Property(role => role.CreatedAt)
                .IsRequired();
            builder.HasIndex(role => role.Name)
                .IsUnique();
        });

        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("Users");
            builder.Property(user => user.Username)
                .IsRequired()
                .HasMaxLength(256);
            builder.Property(user => user.Email)
                .IsRequired()
                .HasMaxLength(320);
            builder.Property(user => user.PasswordHash)
                .IsRequired();

            builder.HasOne(user => user.Role)
                .WithMany(role => role.Users)
                .HasForeignKey(user => user.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

