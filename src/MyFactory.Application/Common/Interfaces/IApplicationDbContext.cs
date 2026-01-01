using Microsoft.EntityFrameworkCore;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Products;
using MyFactory.Domain.Entities.Orders;
using MyFactory.Domain.Entities.Parties;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Entities.Organization;
using MyFactory.Domain.Entities.Finance;

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
    DbSet<FinishedGoodsEntity> FinishedGoods { get; }
    DbSet<FinishedGoodsStockEntity> FinishedGoodsStocks { get; }
    DbSet<FinishedGoodsMovementEntity> FinishedGoodsMovements { get; }
    DbSet<FinishedGoodsMovementItemEntity> FinishedGoodsMovementItems { get; }

    // Orders
    DbSet<SalesOrderEntity> SalesOrders { get; }
    DbSet<SalesOrderItemEntity> SalesOrderItems { get; }
    DbSet<CustomerEntity> Customers { get; }

    // Production
    DbSet<ProductionOrderEntity> ProductionOrders { get; }

    // Contacts
    DbSet<ContactEntity> Contacts { get; }
    DbSet<ContactLinkEntity> ContactLinks { get; }

    // Production Operations
    DbSet<CuttingOperationEntity> CuttingOperations { get; }
    DbSet<SewingOperationEntity> SewingOperations { get; }
    DbSet<PackagingOperationEntity> PackagingOperations { get; }
    DbSet<EmployeeEntity> Employees { get; }
    //DbSet<ProductionOrderDepartmentEmployeeEntity> ProductionOrderDepartmentEmployees { get; }

    // Organization
    DbSet<PositionEntity> Positions { get; }
    DbSet<DepartmentEntity> Departments { get; }
    DbSet<TimesheetEntity> Timesheets { get; }

    // Finance
    DbSet<PayrollAccrualEntity> PayrollAccruals { get; }
    DbSet<PayrollPaymentEntity> PayrollPayments { get; }
    DbSet<PayrollRuleEntity> PayrollRules { get; }
    DbSet<ExpenseTypeEntity> ExpenseTypes { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

