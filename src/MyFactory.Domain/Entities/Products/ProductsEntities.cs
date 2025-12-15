using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Domain.Entities.Products;

public class ProductEntity : AuditableEntity
{
	public string Sku { get; private set; }
	public string Name { get; private set; }
	public int? Version { get; private set; }
	public string? Description { get; private set; }
	public ProductStatus Status { get; private set; }
	public decimal? PlanPerHour { get; private set; }

	// Navigation properties
	public IReadOnlyCollection<ProductDepartmentCostEntity> ProductDepartmentCosts { get; private set; } = new List<ProductDepartmentCostEntity>();
	public IReadOnlyCollection<ProductMaterialEntity> ProductMaterials { get; private set; } = new List<ProductMaterialEntity>();
	public IReadOnlyCollection<FinishedGoodsEntity> FinishedGoods { get; private set; } = new List<FinishedGoodsEntity>();
	public IReadOnlyCollection<WarehouseProductEntity> WarehouseProducts { get; private set; } = new List<WarehouseProductEntity>();
	public IReadOnlyCollection<FinishedGoodsMovementItemEntity> FinishedGoodsMovementItems { get; private set; } = new List<FinishedGoodsMovementItemEntity>();
	public IReadOnlyCollection<ShipmentItemEntity> ShipmentItems { get; private set; } = new List<ShipmentItemEntity>();
	public IReadOnlyCollection<FinishedGoodsStockEntity> FinishedGoodsStocks { get; private set; } = new List<FinishedGoodsStockEntity>();
	public IReadOnlyCollection<ShipmentReturnItemEntity> ShipmentReturnItems { get; private set; } = new List<ShipmentReturnItemEntity>();

	public ProductEntity(
		string sku,
		string name,
		ProductStatus status,
		int? version = null,
		string? description = null,
		decimal? planPerHour = null)
	{
		Guard.AgainstNullOrWhiteSpace(sku, "SKU is required.");
		Guard.AgainstNullOrWhiteSpace(name, "Product name is required.");
		Guard.AgainstNull(status, "Status is required.");
		if (planPerHour.HasValue)
			Guard.AgainstNonPositive(planPerHour.Value, "PlanPerHour must be positive if specified.");
		Sku = sku;
		Name = name;
		Status = status;
		Version = version;
		Description = description;
		PlanPerHour = planPerHour;
	}
}

public enum ProductStatus
{
	Active,
	Inactive,
	Development,
	Discontinued
}

public class ProductMaterialEntity : BaseEntity
{
	public Guid ProductId { get; private set; }
	public Guid MaterialId { get; private set; }
	public decimal QtyPerUnit { get; private set; }

	// Navigation properties
	public ProductEntity? Product { get; private set; }
	public MaterialEntity? Material { get; private set; }

	public ProductMaterialEntity(Guid productId, Guid materialId, decimal qtyPerUnit)
	{
		Guard.AgainstEmptyGuid(productId, "ProductId is required.");
		Guard.AgainstEmptyGuid(materialId, "MaterialId is required.");
		Guard.AgainstNonPositive(qtyPerUnit, "QtyPerUnit must be positive.");
		ProductId = productId;
		MaterialId = materialId;
		QtyPerUnit = qtyPerUnit;
	}
}

public class ProductDepartmentCostEntity : AuditableEntity
{
	public Guid ProductId { get; private set; }
	public Guid DepartmentId { get; private set; }
	public decimal ExpensesPerUnit { get; private set; }
	public decimal CutCostPerUnit { get; private set; }
	public decimal SewingCostPerUnit { get; private set; }
	public decimal PackCostPerUnit { get; private set; }
	public bool IsActive { get; private set; }

	// Navigation properties
	public ProductEntity? Product { get; private set; }

	public ProductDepartmentCostEntity(
		Guid productId,
		Guid departmentId,
		decimal expensesPerUnit,
		decimal cutCostPerUnit,
		decimal sewingCostPerUnit,
		decimal packCostPerUnit,
		bool isActive = true)
	{
		Guard.AgainstEmptyGuid(productId, "ProductId is required.");
		Guard.AgainstEmptyGuid(departmentId, "DepartmentId is required.");
		Guard.AgainstNegative(expensesPerUnit, "ExpensesPerUnit cannot be negative.");
		Guard.AgainstNegative(cutCostPerUnit, "CutCostPerUnit cannot be negative.");
		Guard.AgainstNegative(sewingCostPerUnit, "SewingCostPerUnit cannot be negative.");
		Guard.AgainstNegative(packCostPerUnit, "PackCostPerUnit cannot be negative.");
		ProductId = productId;
		DepartmentId = departmentId;
		ExpensesPerUnit = expensesPerUnit;
		CutCostPerUnit = cutCostPerUnit;
		SewingCostPerUnit = sewingCostPerUnit;
		PackCostPerUnit = packCostPerUnit;
		IsActive = isActive;
	}
}