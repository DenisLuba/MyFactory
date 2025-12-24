using Microsoft.EntityFrameworkCore;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Products;

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

    // Inventory Movements
    DbSet<InventoryMovementEntity> InventoryMovements { get; }
    DbSet<InventoryMovementItemEntity> InventoryMovementItems { get; }

    // Products
    DbSet<ProductEntity> Products { get; }
    DbSet<ProductMaterialEntity> ProductMaterials { get; }
    DbSet<ProductDepartmentCostEntity> ProductDepartmentCosts { get; }

    // Warehouse
    DbSet<FinishedGoodsStockEntity> FinishedGoodsStocks { get; }
    DbSet<FinishedGoodsMovementEntity> FinishedGoodsMovements { get; }
    DbSet<FinishedGoodsMovementItemEntity> FinishedGoodsMovementItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

