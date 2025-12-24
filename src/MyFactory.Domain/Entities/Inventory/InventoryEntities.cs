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
	public decimal Qty { get; private set; }

	// Navigation properties
	public WarehouseEntity? Warehouse { get; private set; }
	// MaterialEntity is assumed to exist in the domain model
	public MaterialEntity? Material { get; private set; }

	public WarehouseMaterialEntity(Guid warehouseId, Guid materialId, decimal qty = 0)
	{
		Guard.AgainstEmptyGuid(warehouseId, "WarehouseId is required.");
		Guard.AgainstEmptyGuid(materialId, "MaterialId is required.");
		Guard.AgainstNegative(qty, "Qty cannot be negative.");
		WarehouseId = warehouseId;
		MaterialId = materialId;
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
			throw new DomainException("Cannot remove more material than is available in stock.");
		Qty -= amount;
		Touch();
	}

	public void AdjustQty(decimal newQty)
	{
		Guard.AgainstNegative(newQty, "New quantity cannot be negative.");
		Qty = newQty;
		Touch();
	}
}

public class WarehouseProductEntity : AuditableEntity
{
	public Guid WarehouseId { get; private set; }
	public Guid ProductId { get; private set; }
	public int Qty { get; private set; }

	// Navigation properties
	public WarehouseEntity? Warehouse { get; private set; }
	public ProductEntity? Product { get; private set; }

	public WarehouseProductEntity(Guid warehouseId, Guid productId, int qty = 0)
	{
		Guard.AgainstEmptyGuid(warehouseId, "WarehouseId is required.");
		Guard.AgainstEmptyGuid(productId, "ProductId is required.");
		Guard.AgainstNegative(qty, "Qty cannot be negative.");
		WarehouseId = warehouseId;
		ProductId = productId;
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
			throw new DomainException("Cannot remove more product than is available in stock.");
		Qty -= amount;
		Touch();
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
	public int Qty { get; private set; }

	// Navigation properties
	public WarehouseEntity? Warehouse { get; private set; }
	public ProductEntity? Product { get; private set; }

	public FinishedGoodsStockEntity(Guid warehouseId, Guid productId, int qty = 0)
	{
		Guard.AgainstEmptyGuid(warehouseId, "WarehouseId is required.");
		Guard.AgainstEmptyGuid(productId, "ProductId is required.");
		Guard.AgainstNegative(qty, "Qty cannot be negative.");
		WarehouseId = warehouseId;
		ProductId = productId;
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
		ValidateMovement(movementType, fromWarehouseId, toWarehouseId, toDepartmentId);
		Guard.AgainstNull(movementType, "MovementType is required.");
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
		}
	}
}

public enum InventoryMovementType
{
	IssueToDept,
	ReturnFromDept,
	Transfer,
	Adjustment
}

public class InventoryMovementItemEntity : AuditableEntity
{
	public Guid MovementId { get; private set; }
	public Guid MaterialId { get; private set; }
	public decimal Qty { get; private set; }

	// Navigation properties
	public InventoryMovementEntity? Movement { get; private set; }
	public MaterialEntity? Material { get; private set; }

	public InventoryMovementItemEntity(Guid movementId, Guid materialId, decimal qty)
	{
		Guard.AgainstEmptyGuid(movementId, "MovementId is required.");
		Guard.AgainstEmptyGuid(materialId, "MaterialId is required.");
		Guard.AgainstNegative(qty, "Qty cannot be negative.");
		MovementId = movementId;
		MaterialId = materialId;
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
			throw new DomainException("Cannot remove more material than is available in this movement item.");
		Qty -= amount;
		Touch();
	}
}

public class FinishedGoodsMovementEntity : AuditableEntity
{
	public Guid FromWarehouseId { get; private set; }
	public Guid ToWarehouseId { get; private set; }
	public DateTime MovementDate { get; private set; }
	public Guid CreatedBy { get; private set; }

	// Navigation properties
	public WarehouseEntity? FromWarehouse { get; private set; }
	public WarehouseEntity? ToWarehouse { get; private set; }
	public UserEntity? CreatedByUser { get; private set; }
	public IReadOnlyCollection<FinishedGoodsMovementItemEntity> FinishedGoodsMovementItems { get; private set; } = new List<FinishedGoodsMovementItemEntity>();

	public FinishedGoodsMovementEntity(Guid fromWarehouseId, Guid toWarehouseId, DateTime movementDate, Guid createdBy)
	{
		Guard.AgainstEmptyGuid(fromWarehouseId, "FromWarehouseId is required.");
		Guard.AgainstEmptyGuid(toWarehouseId, "ToWarehouseId is required.");
		Guard.AgainstDefaultDate(movementDate, "MovementDate is required.");
		Guard.AgainstEmptyGuid(createdBy, "CreatedBy is required.");
		FromWarehouseId = fromWarehouseId;
		ToWarehouseId = toWarehouseId;
		MovementDate = movementDate;
		CreatedBy = createdBy;
	}
}

public class FinishedGoodsMovementItemEntity : AuditableEntity
{
	public Guid MovementId { get; private set; }
	public Guid ProductId { get; private set; }
	public int Qty { get; private set; }

	// Navigation properties
	public FinishedGoodsMovementEntity? Movement { get; private set; }
	public ProductEntity? Product { get; private set; }

	public FinishedGoodsMovementItemEntity(Guid movementId, Guid productId, int qty)
	{
		Guard.AgainstEmptyGuid(movementId, "MovementId is required.");
		Guard.AgainstEmptyGuid(productId, "ProductId is required.");
		Guard.AgainstNegative(qty, "Qty cannot be negative.");
		MovementId = movementId;
		ProductId = productId;
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
			throw new DomainException("Cannot remove more product than is available in this movement item.");
		Qty -= amount;
		Touch();
	}
}