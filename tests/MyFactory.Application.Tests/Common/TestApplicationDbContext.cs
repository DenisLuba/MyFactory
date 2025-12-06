using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Identity;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.Tests.Common;

internal sealed class TestApplicationDbContext : DbContext, IApplicationDbContext
{
    public TestApplicationDbContext(DbContextOptions<TestApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Material> Materials => Set<Material>();

    public DbSet<MaterialType> MaterialTypes => Set<MaterialType>();

    public DbSet<Supplier> Suppliers => Set<Supplier>();

    public DbSet<MaterialPriceHistory> MaterialPriceHistoryEntries => Set<MaterialPriceHistory>();

    public DbSet<Warehouse> Warehouses => Set<Warehouse>();

    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();

    public DbSet<InventoryReceipt> InventoryReceipts => Set<InventoryReceipt>();

    public DbSet<InventoryReceiptItem> InventoryReceiptItems => Set<InventoryReceiptItem>();

    public DbSet<PurchaseRequest> PurchaseRequests => Set<PurchaseRequest>();

    public DbSet<PurchaseRequestItem> PurchaseRequestItems => Set<PurchaseRequestItem>();
}
