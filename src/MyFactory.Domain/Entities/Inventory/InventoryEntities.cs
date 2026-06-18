using System.ComponentModel.DataAnnotations.Schema;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Orders;
using MyFactory.Domain.Entities.Organization;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Entities.Products;
using MyFactory.Domain.Entities.Security;
using MyFactory.Domain.Exceptions;

namespace MyFactory.Domain.Entities.Inventory;

public class WarehouseEntity : ActivatableEntity
{
    public string Name { get; private set; }
    public WarehouseType Type { get; private set; }

    // Navigation properties
    public IReadOnlyCollection<WarehouseMaterialEntity> WarehouseMaterials { get; private set; } = new List<WarehouseMaterialEntity>();
    public IReadOnlyCollection<InventoryMovementEntity> InventoryMovementsFrom { get; private set; } = new List<InventoryMovementEntity>();
    public IReadOnlyCollection<InventoryMovementEntity> InventoryMovementsTo { get; private set; } = new List<InventoryMovementEntity>();
    public IReadOnlyCollection<FinishedGoodsEntity> FinishedGoods { get; private set; } = new List<FinishedGoodsEntity>();
    public IReadOnlyCollection<WarehouseProductEntity> WarehouseProducts { get; private set; } = new List<WarehouseProductEntity>();
    public IReadOnlyCollection<FinishedGoodsMovementEntity> FinishedGoodsMovementsFrom { get; private set; } = new List<FinishedGoodsMovementEntity>();
    public IReadOnlyCollection<FinishedGoodsMovementEntity> FinishedGoodsMovementsTo { get; private set; } = new List<FinishedGoodsMovementEntity>();
    public IReadOnlyCollection<ShipmentItemEntity> ShipmentItems { get; private set; } = new List<ShipmentItemEntity>();
    public IReadOnlyCollection<FinishedGoodsStockEntity> FinishedGoodsStocks { get; private set; } = new List<FinishedGoodsStockEntity>();
    public IReadOnlyCollection<ShipmentReturnItemEntity> ShipmentReturnItems { get; private set; } = new List<ShipmentReturnItemEntity>();
    public ICollection<FinishedGoodsScrapEntity> FinishedGoodsScraps { get; private set; } = new List<FinishedGoodsScrapEntity>();

    public WarehouseEntity(string name, WarehouseType type)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Warehouse name is required.");
        Name = name;
        Type = type;
    }

    public void Update(string name, WarehouseType type)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Warehouse name is required.");
        Name = name;
        Type = type;
        Touch();
    }
}

public class FinishedGoodsScrapEntity : AuditableEntity
{
    public Guid WarehouseId { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid? ProductionOrderId { get; private set; }
    public int Qty { get; private set; }
    public DateOnly ScrapDate { get; private set; }
    public string? Reason { get; private set; }
    public Guid CreatedBy { get; private set; }

    public WarehouseEntity? Warehouse { get; private set; }
    public ProductEntity? Product { get; private set; }
    public ProductionOrderEntity? ProductionOrder { get; private set; }
    public UserEntity? CreatedByUser { get; private set; }

    public FinishedGoodsScrapEntity(
        Guid warehouseId,
        Guid productId,
        int qty,
        DateOnly scrapDate,
        Guid createdBy,
        Guid? productionOrderId = null,
        string? reason = null)
    {
        Guard.AgainstEmptyGuid(warehouseId, nameof(warehouseId));
        Guard.AgainstEmptyGuid(productId, nameof(productId));
        Guard.AgainstNonPositive(qty, nameof(qty));
        Guard.AgainstDefaultDate(scrapDate, nameof(scrapDate));
        Guard.AgainstEmptyGuid(createdBy, nameof(createdBy));

        WarehouseId = warehouseId;
        ProductId = productId;
        Qty = qty;
        ScrapDate = scrapDate;
        CreatedBy = createdBy;
        ProductionOrderId = productionOrderId;
        Reason = reason;
    }

    public void UpdateReason(string? reason)
    {
        Reason = reason;
        Touch();
    }

    public void UpdateQty(int qty)
    {
        Guard.AgainstNonPositive(qty, nameof(qty));
        Qty = qty;
        Touch();
    }
}

public enum WarehouseType
{
    Materials,
    FinishedGoods,
    Aux
}

public class WarehouseMaterialEntity : AuditableEntity
{
    public Guid WarehouseId { get; private set; }
    public Guid MaterialId { get; private set; }

    public decimal QtyPerPackage { get; private set; }
    public decimal? PackageCount { get; private set; }
    public decimal Qty { get; private set; }

    // Navigation properties
    public WarehouseEntity? Warehouse { get; private set; }
    public MaterialEntity? Material { get; private set; }

    public WarehouseMaterialEntity(Guid warehouseId, Guid materialId, decimal qtyPerPackage, decimal? packageCount = null)
    {
        Guard.AgainstEmptyGuid(warehouseId, "WarehouseId is required.");
        Guard.AgainstEmptyGuid(materialId, "MaterialId is required.");
        Guard.AgainstNegative(qtyPerPackage, "Qty cannot be negative.");
        if (packageCount is not null)
        {
            Guard.AgainstNegative(packageCount ?? 0m, "Package count cannot be negative.");
        }
        WarehouseId = warehouseId;
        MaterialId = materialId;
        QtyPerPackage = qtyPerPackage;
        PackageCount = NormalizePackageCount(packageCount);
        Qty = CalculateTotal(QtyPerPackage, PackageCount);
    }

    public void AddQty(decimal amount, decimal? packageCount = null)
    {
        Guard.AgainstNonPositive(amount, "Amount to add must be positive.");
        if (packageCount is not null)
        {
            Guard.AgainstNonPositive(packageCount ?? 0m, "Package count must be positive.");
        }

        var additionalTotal = CalculateTotal(amount, packageCount);
        var newTotal = Qty + additionalTotal;

        if (PackageCount is null && packageCount is null)
        {
            QtyPerPackage = newTotal;
        }

        else if (PackageCount is null && packageCount is not null)
        {
            // Convert existing total qty into packages of the incoming size.
            var existingPackages = Qty == 0m ? 0m : Qty / amount;
            QtyPerPackage = amount;
            PackageCount = existingPackages + packageCount;
        }

        else if (PackageCount is not null && packageCount is not null)
        {
            Guard.AgainstNonPositive(QtyPerPackage, "The quantity per package must be positive.");

            if (QtyPerPackage == amount)
            {
                PackageCount += packageCount;
            }
            else
            {
                throw new DomainException("The quantity in the package does not match the existing quantity in the package.");
            }
        }

        else if (PackageCount is not null && packageCount is null)
        {
            Guard.AgainstNonPositive(QtyPerPackage, "The quantity per package must be positive.");

            var additionalPackages = amount / QtyPerPackage;
            PackageCount += additionalPackages;
        }

        Qty = newTotal;

        Touch();
    }

    public void RemoveQty(decimal amount, decimal? packageCount = null)
    {
        Guard.AgainstNonPositive(amount, "Amount to remove must be positive.");
        if (packageCount is not null)
        {
            Guard.AgainstNonPositive(packageCount ?? 0m, "Package count must be positive.");
        }

        var removalTotal = CalculateTotal(amount, packageCount);
        if (removalTotal > Qty)
            throw new DomainException("Cannot remove more material than is available in stock.");

        var newTotal = Qty - removalTotal;

        if (PackageCount is null && packageCount is null)
        {
            QtyPerPackage = newTotal;
        }

        else if (PackageCount is null && packageCount is not null)
        {
            var existingPackages = Qty / amount;
            var newPackageCount = existingPackages - packageCount;

            if (newPackageCount < 0m)
                throw new DomainException("Cannot remove more packages than are available.");

            QtyPerPackage = amount;
            PackageCount = newPackageCount <= 0m ? null : newPackageCount;
            if (PackageCount is null)
            {
                QtyPerPackage = newTotal;
            }
        }

        else if (PackageCount is not null && packageCount is not null)
        {
            Guard.AgainstNonPositive(QtyPerPackage, "The quantity per package must be positive.");

            if (packageCount > PackageCount)
            {
                throw new DomainException("Cannot remove more packages than are available.");
            }

            if (QtyPerPackage == amount)
            {
                PackageCount -= packageCount;
                if (PackageCount <= 0)
                {
                    PackageCount = null;
                    QtyPerPackage = newTotal;
                }
            }
            else
            {
                throw new DomainException("The quantity in the package does not match the existing quantity in the package.");
            }
        }

        else if (PackageCount is not null && packageCount is null)
        {
            Guard.AgainstNonPositive(QtyPerPackage, "The quantity per package must be positive.");

            var removalPackages = amount / QtyPerPackage;
            if (removalPackages > PackageCount)
            {
                throw new DomainException("Cannot remove more packages than are available.");
            }
            PackageCount -= removalPackages;
            if (PackageCount <= 0)
            {
                PackageCount = null;
                QtyPerPackage = newTotal;
            }
        }

        Qty = newTotal;

        Touch();
    }

    public void AdjustQty(decimal newQty, decimal? packageCount = null)
    {
        Guard.AgainstNegative(newQty, "New quantity cannot be negative.");
        if (packageCount is not null)
        {
            Guard.AgainstNegative(packageCount ?? 0m, "Package count cannot be negative.");
        }

        var normalizedPackageCount = NormalizePackageCount(packageCount);
        QtyPerPackage = newQty;
        PackageCount = normalizedPackageCount;
        Qty = CalculateTotal(QtyPerPackage, PackageCount);

        Touch();
    }

    private static decimal CalculateTotal(decimal amount, decimal? packageCount)
        => packageCount is null ? amount : amount * packageCount.Value;

    private static decimal? NormalizePackageCount(decimal? packageCount)
        => packageCount is > 0m ? packageCount : null;
}

public class WarehouseProductEntity : AuditableEntity
{
    public Guid WarehouseId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Qty { get; private set; }
    public int? PackageCount { get; private set; }

    [NotMapped]
    public int TotalQty => CalculateTotal(Qty, PackageCount);

    // Navigation properties
    public WarehouseEntity? Warehouse { get; private set; }
    public ProductEntity? Product { get; private set; }

    public WarehouseProductEntity(Guid warehouseId, Guid productId, int qty = 0, int? packageCount = null)
    {
        Guard.AgainstEmptyGuid(warehouseId, "WarehouseId is required.");
        Guard.AgainstEmptyGuid(productId, "ProductId is required.");
        Guard.AgainstNegative(qty, "Qty cannot be negative.");
        if (packageCount is not null)
        {
            Guard.AgainstNegative(packageCount ?? 0, "Package count cannot be negative.");
        }
        WarehouseId = warehouseId;
        ProductId = productId;
        Qty = qty;
        PackageCount = packageCount;
    }

    public void AddQty(int amount, int? packageCount = null)
    {
        Guard.AgainstNonPositive(amount, "Amount to add must be positive.");
        if (packageCount is not null)
        {
            Guard.AgainstNonPositive(packageCount ?? 0, "Package count must be positive.");
        }

        var additionalTotal = CalculateTotal(amount, packageCount);
        var newTotal = TotalQty + additionalTotal;

        if (PackageCount is null && packageCount is null)
        {
            Qty = newTotal;
        }
        else if (PackageCount is not null && packageCount is not null && Qty == amount)
        {
            PackageCount += packageCount;
        }
        else if (PackageCount is null && packageCount is not null)
        {
            Qty = amount;
            PackageCount = packageCount;
        }
        else
        {
            FlattenToTotal(newTotal);
        }

        Touch();
    }

    public void RemoveQty(int amount, int? packageCount = null)
    {
        Guard.AgainstNonPositive(amount, "Amount to remove must be positive.");
        if (packageCount is not null)
        {
            Guard.AgainstNonPositive(packageCount ?? 0, "Package count must be positive.");
        }

        var removalTotal = CalculateTotal(amount, packageCount);
        if (removalTotal > TotalQty)
            throw new DomainException("Cannot remove more product than is available in stock.");

        var newTotal = TotalQty - removalTotal;

        if (PackageCount is not null && packageCount is not null && Qty == amount)
        {
            PackageCount -= packageCount;
            if (PackageCount <= 0)
            {
                FlattenToTotal(newTotal);
            }
        }
        else if (PackageCount is null && packageCount is null)
        {
            Qty = newTotal;
        }
        else
        {
            FlattenToTotal(newTotal);
        }

        Touch();
    }

    public void AdjustQty(int newQty, int? packageCount = null)
    {
        Guard.AgainstNegative(newQty, "New quantity cannot be negative.");
        if (packageCount is not null)
        {
            Guard.AgainstNegative(packageCount ?? 0, "Package count cannot be negative.");
        }

        if (packageCount is not null && packageCount <= 0)
        {
            PackageCount = null;
            Qty = newQty;
        }
        else
        {
            Qty = newQty;
            PackageCount = packageCount;
        }

        Touch();
    }

    private static int CalculateTotal(int quantityPerPackage, int? packageCount) => packageCount is null ? quantityPerPackage : quantityPerPackage * packageCount.Value;

    private void FlattenToTotal(int totalQty)
    {
        Qty = totalQty;
        PackageCount = null;
    }
}

public class FinishedGoodsEntity : AuditableEntity
{
    public Guid ProductId { get; private set; }
    public Guid WarehouseId { get; private set; }
    public Guid ProductionOrderId { get; private set; }
    public int Qty { get; private set; }

    // Navigation properties
    public ProductEntity? Product { get; private set; }
    public WarehouseEntity? Warehouse { get; private set; }
    public ProductionOrderEntity? ProductionOrder { get; private set; }

    public FinishedGoodsEntity(Guid productId, Guid warehouseId, Guid productionOrderId, int qty)
    {
        Guard.AgainstEmptyGuid(productId, "ProductId is required.");
        Guard.AgainstEmptyGuid(warehouseId, "WarehouseId is required.");
        Guard.AgainstEmptyGuid(productionOrderId, "ProductionOrderId is required.");
        Guard.AgainstNegative(qty, "Qty cannot be negative.");
        ProductId = productId;
        WarehouseId = warehouseId;
        ProductionOrderId = productionOrderId;
        Qty = qty;
    }

    public void AddQty(int amount)
    {
        Guard.AgainstNonPositive(amount, "Amount to add must be positive.");
        Qty += amount;
        Touch();
    }

    public void RemoveQty(int amount)
    {
        Guard.AgainstNonPositive(amount, "Amount to remove must be positive.");
        if (amount > Qty)
            throw new DomainException("Cannot remove more finished goods than are available in stock.");
        Qty -= amount;
        Touch();
    }
}

public class FinishedGoodsStockEntity : AuditableEntity
{
    public Guid WarehouseId { get; private set; }
    public Guid ProductId { get; private set; }
    public decimal QtyPerPackage { get; private set; }
    public decimal? PackageCount { get; private set; }
    public decimal Qty { get; private set; }

    // Navigation properties
    public WarehouseEntity? Warehouse { get; private set; }
    public ProductEntity? Product { get; private set; }

    public FinishedGoodsStockEntity(Guid warehouseId, Guid productId, decimal qtyPerPackage, decimal? packageCount = null)
    {
        Guard.AgainstEmptyGuid(warehouseId, "WarehouseId is required.");
        Guard.AgainstEmptyGuid(productId, "ProductId is required.");
        Guard.AgainstNegative(qtyPerPackage, "Qty cannot be negative.");
        if (packageCount is not null)
        {
            Guard.AgainstNegative(packageCount ?? 0m, "Package count cannot be negative.");
        }
        WarehouseId = warehouseId;
        ProductId = productId;
        QtyPerPackage = qtyPerPackage;
        PackageCount = NormalizePackageCount(packageCount);
        Qty = CalculateTotal(QtyPerPackage, PackageCount);
    }

    public void AddQty(decimal amount, decimal? packageCount = null)
    {
        Guard.AgainstNonPositive(amount, "Amount to add must be positive.");
        if (packageCount is not null)
        {
            Guard.AgainstNonPositive(packageCount ?? 0m, "Package count must be positive.");
        }

        var additionalTotal = CalculateTotal(amount, packageCount);
        var newTotal = Qty + additionalTotal;

        if (PackageCount is null && packageCount is null)
        {
            QtyPerPackage = newTotal;
        }

        else if (PackageCount is null && packageCount is not null)
        {
            // Convert existing total qty into packages of the incoming size.
            var existingPackages = Qty == 0m ? 0m : Qty / amount;
            QtyPerPackage = amount;
            PackageCount = existingPackages + packageCount;
        }

        else if (PackageCount is not null && packageCount is not null)
        {
            Guard.AgainstNonPositive(QtyPerPackage, "The quantity per package must be positive.");

            if (QtyPerPackage == amount)
            {
                PackageCount += packageCount;
            }
            else
            {
                throw new DomainException("The quantity in the package does not match the existing quantity in the package.");
            }
        }

        else if (PackageCount is not null && packageCount is null)
        {
            Guard.AgainstNonPositive(QtyPerPackage, "The quantity per package must be positive.");

            var additionalPackages = amount / QtyPerPackage;
            PackageCount += additionalPackages;
        }

        Qty = newTotal;

        Touch();
    }

    public void RemoveQty(decimal amount, decimal? packageCount = null)
    {
        Guard.AgainstNonPositive(amount, "Amount to remove must be positive.");
        if (packageCount is not null)
        {
            Guard.AgainstNonPositive(packageCount ?? 0m, "Package count must be positive.");
        }

        var removalTotal = CalculateTotal(amount, packageCount);
        if (removalTotal > Qty)
            throw new DomainException("Cannot remove more material than is available in stock.");

        var newTotal = Qty - removalTotal;

        if (PackageCount is null && packageCount is null)
        {
            QtyPerPackage = newTotal;
        }

        else if (PackageCount is null && packageCount is not null)
        {
            var existingPackages = Qty / amount;
            var newPackageCount = existingPackages - packageCount;

            if (newPackageCount < 0m)
                throw new DomainException("Cannot remove more packages than are available.");

            QtyPerPackage = amount;
            PackageCount = newPackageCount <= 0m ? null : newPackageCount;
            if (PackageCount is null)
            {
                QtyPerPackage = newTotal;
            }
        }

        else if (PackageCount is not null && packageCount is not null)
        {
            Guard.AgainstNonPositive(QtyPerPackage, "The quantity per package must be positive.");

            if (packageCount > PackageCount)
            {
                throw new DomainException("Cannot remove more packages than are available.");
            }

            if (QtyPerPackage == amount)
            {
                PackageCount -= packageCount;
                if (PackageCount <= 0)
                {
                    PackageCount = null;
                    QtyPerPackage = newTotal;
                }
            }
            else
            {
                throw new DomainException("The quantity in the package does not match the existing quantity in the package.");
            }
        }

        else if (PackageCount is not null && packageCount is null)
        {
            Guard.AgainstNonPositive(QtyPerPackage, "The quantity per package must be positive.");

            var removalPackages = amount / QtyPerPackage;
            if (removalPackages > PackageCount)
            {
                throw new DomainException("Cannot remove more packages than are available.");
            }
            PackageCount -= removalPackages;
            if (PackageCount <= 0)
            {
                PackageCount = null;
                QtyPerPackage = newTotal;
            }
        }

        Qty = newTotal;

        Touch();
    }

    public void AdjustQty(decimal newQty, decimal? packageCount = null)
    {
        Guard.AgainstNegative(newQty, "New quantity cannot be negative.");
        if (packageCount is not null)
        {
            Guard.AgainstNegative(packageCount ?? 0m, "Package count cannot be negative.");
        }

        var normalizedPackageCount = NormalizePackageCount(packageCount);
        QtyPerPackage = newQty;
        PackageCount = normalizedPackageCount;
        Qty = CalculateTotal(QtyPerPackage, PackageCount);

        Touch();
    }

    private static decimal CalculateTotal(decimal amount, decimal? packageCount)
        => packageCount is null ? amount : amount * packageCount.Value;

    private static decimal? NormalizePackageCount(decimal? packageCount)
        => packageCount is > 0m ? packageCount : null;
}

public class InventoryMovementEntity : AuditableEntity
{
    public InventoryMovementType MovementType { get; private set; }
    public Guid? FromWarehouseId { get; private set; }
    public Guid? ToWarehouseId { get; private set; }
    public Guid? ToDepartmentId { get; private set; }
    public Guid? ProductionOrderId { get; private set; }
    public Guid CreatedBy { get; private set; }

    // Navigation properties
    public WarehouseEntity? FromWarehouse { get; private set; }
    public WarehouseEntity? ToWarehouse { get; private set; }
    public DepartmentEntity? ToDepartment { get; private set; }
    public ProductionOrderEntity? ProductionOrder { get; private set; }
    public UserEntity? CreatedByUser { get; private set; }
    public IReadOnlyCollection<InventoryMovementItemEntity> InventoryMovementItems { get; private set; } = new List<InventoryMovementItemEntity>();

    public InventoryMovementEntity(
        InventoryMovementType movementType,
        Guid? fromWarehouseId,
        Guid? toWarehouseId,
        Guid? toDepartmentId,
        Guid? productionOrderId,
        Guid createdBy)
    {
        Guard.AgainstNull(movementType, "MovementType is required.");
        ValidateMovement(movementType, fromWarehouseId, toWarehouseId, toDepartmentId);
        Guard.AgainstEmptyGuid(createdBy, "CreatedBy is required.");

        MovementType = movementType;
        FromWarehouseId = fromWarehouseId;
        ToWarehouseId = toWarehouseId;
        ToDepartmentId = toDepartmentId;
        ProductionOrderId = productionOrderId;
        CreatedBy = createdBy;
    }

    private static void ValidateMovement(
        InventoryMovementType movementType,
        Guid? fromWarehouseId,
        Guid? toWarehouseId,
        Guid? toDepartmentId)
    {
        switch (movementType)
        {
            case InventoryMovementType.IssueToDept:
            case InventoryMovementType.ReturnFromDept:
                Guard.AgainstNull(fromWarehouseId, nameof(fromWarehouseId));
                Guard.AgainstNull(toDepartmentId, nameof(toDepartmentId));
                break;

            case InventoryMovementType.Transfer:
                Guard.AgainstNull(fromWarehouseId, nameof(fromWarehouseId));
                Guard.AgainstNull(toWarehouseId, nameof(toWarehouseId));
                break;

            case InventoryMovementType.Adjustment:
                // допускаем всё null
                break;

            case InventoryMovementType.Receipt:
                Guard.AgainstNull(toWarehouseId, nameof(toWarehouseId));
                break;
        }
    }
}

public enum InventoryMovementType
{
    IssueToDept,
    ReturnFromDept,
    Transfer,
    Adjustment,
    Receipt
}

public class InventoryMovementItemEntity : AuditableEntity
{
    public Guid MovementId { get; private set; }
    public Guid MaterialId { get; private set; }
    public decimal Qty { get; private set; }
    public decimal UnitCost { get; private set; }

    // Navigation properties
    public InventoryMovementEntity? Movement { get; private set; }
    public MaterialEntity? Material { get; private set; }

    public InventoryMovementItemEntity(Guid movementId, Guid materialId, decimal qty, decimal unitCost)
    {
        Guard.AgainstEmptyGuid(movementId, "MovementId is required.");
        Guard.AgainstEmptyGuid(materialId, "MaterialId is required.");
        Guard.AgainstNegative(qty, "Qty cannot be negative.");
        Guard.AgainstNegative(unitCost, "UnitCost cannot be negative.");

        MovementId = movementId;
        MaterialId = materialId;
        Qty = qty;
        UnitCost = unitCost;
    }

    public void AddQty(decimal amount)
    {
        Guard.AgainstNonPositive(amount, "Amount to add must be positive.");
        Qty += amount;
        Touch();
    }

    public void RemoveQty(decimal amount)
    {
        Guard.AgainstNonPositive(amount, "Amount to remove must be positive.");
        if (amount > Qty)
            throw new DomainException("Cannot remove more material than is available in this movement item.");
        Qty -= amount;
        Touch();
    }
}

public class FinishedGoodsMovementEntity : AuditableEntity
{
    public FinishedGoodsMovementType MovementType { get; private set; }
    public Guid? FromWarehouseId { get; private set; }
    public Guid? ToWarehouseId { get; private set; }
    public DateTime MovementDate { get; private set; }
    public Guid CreatedBy { get; private set; }

    // Navigation properties
    public WarehouseEntity? FromWarehouse { get; private set; }
    public WarehouseEntity? ToWarehouse { get; private set; }
    public UserEntity? CreatedByUser { get; private set; }
    public IReadOnlyCollection<FinishedGoodsMovementItemEntity> FinishedGoodsMovementItems { get; private set; } = new List<FinishedGoodsMovementItemEntity>();

    public FinishedGoodsMovementEntity(FinishedGoodsMovementType movementType, Guid? fromWarehouseId, Guid? toWarehouseId, DateTime movementDate, Guid createdBy)
    {
        Guard.AgainstNull(movementType, "Type is required.");
        ValidateMovement(movementType, fromWarehouseId, toWarehouseId);
        Guard.AgainstDefaultDate(movementDate, "MovementDate is required.");
        Guard.AgainstEmptyGuid(createdBy, "CreatedBy is required.");
        MovementType = movementType;
        FromWarehouseId = fromWarehouseId;
        ToWarehouseId = toWarehouseId;
        MovementDate = movementDate;
        CreatedBy = createdBy;
    }

    private static void ValidateMovement(
        FinishedGoodsMovementType movementType,
        Guid? fromWarehouseId,
        Guid? toWarehouseId)
    {
        switch (movementType)
        {
            case FinishedGoodsMovementType.Shipment:
                 Guard.AgainstNull(fromWarehouseId, nameof(fromWarehouseId));
                break;
            case FinishedGoodsMovementType.Sale:
                Guard.AgainstNull(fromWarehouseId, nameof(fromWarehouseId));
                break;
            case FinishedGoodsMovementType.Return:
                Guard.AgainstNull(toWarehouseId, nameof(toWarehouseId));
                break;
            case FinishedGoodsMovementType.Transfer:
                Guard.AgainstNull(fromWarehouseId, nameof(fromWarehouseId));
                Guard.AgainstNull(toWarehouseId, nameof(toWarehouseId));
                break;

            case FinishedGoodsMovementType.Adjustment:
                // допускаем всё null
                break;
        }
    }
}

public enum FinishedGoodsMovementType
{
    Shipment,
    Sale,
    Transfer,
    Adjustment,
    Return
}

public class FinishedGoodsMovementItemEntity : AuditableEntity
{
    public Guid MovementId { get; private set; }
    public Guid ProductId { get; private set; }
    public decimal Qty { get; private set; }

    // Navigation properties
    public FinishedGoodsMovementEntity? Movement { get; private set; }
    public ProductEntity? Product { get; private set; }

    public FinishedGoodsMovementItemEntity(Guid movementId, Guid productId, decimal qty)
    {
        Guard.AgainstEmptyGuid(movementId, "MovementId is required.");
        Guard.AgainstEmptyGuid(productId, "ProductId is required.");
        Guard.AgainstNegative(qty, "Qty cannot be negative.");
        MovementId = movementId;
        ProductId = productId;
        Qty = qty;
    }

    public void AddQty(decimal amount)
    {
        Guard.AgainstNonPositive(amount, "Amount to add must be positive.");
        Qty += amount;
        Touch();
    }

    public void RemoveQty(decimal amount)
    {
        Guard.AgainstNonPositive(amount, "Amount to remove must be positive.");
        if (amount > Qty)
            throw new DomainException("Cannot remove more product than is available in this movement item.");
        Qty -= amount;
        Touch();
    }
}