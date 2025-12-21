using Microsoft.EntityFrameworkCore;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    // Materials List
    DbSet<MaterialEntity> Materials { get; }
    DbSet<MaterialTypeEntity> MaterialTypes { get; }
    DbSet<UnitEntity> Units { get; }
    DbSet<WarehouseMaterialEntity> WarehouseMaterials { get; }

    // Material Details
    DbSet<WarehouseEntity> Warehouses { get; }
    DbSet<MaterialPurchaseOrderEntity> MaterialPurchaseOrders { get; }
    DbSet<MaterialPurchaseOrderItemEntity> MaterialPurchaseOrderItems { get; }
    DbSet<SupplierEntity> Suppliers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

