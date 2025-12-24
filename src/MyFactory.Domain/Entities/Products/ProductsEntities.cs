using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Orders;
using MyFactory.Domain.Entities.Organization;

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
	public IReadOnlyCollection<ProductDepartmentCostEntity> ProductDepartmentCosts { get; private set; } = [];
	public IReadOnlyCollection<ProductMaterialEntity> ProductMaterials { get; private set; } = [];
    public IReadOnlyCollection<FinishedGoodsEntity> FinishedGoods { get; private set; } = [];
    public IReadOnlyCollection<WarehouseProductEntity> WarehouseProducts { get; private set; } = [];
    public IReadOnlyCollection<FinishedGoodsMovementItemEntity> FinishedGoodsMovementItems { get; private set; } = [];
    public IReadOnlyCollection<ShipmentItemEntity> ShipmentItems { get; private set; } = [];
    public IReadOnlyCollection<FinishedGoodsStockEntity> FinishedGoodsStocks { get; private set; } = [];
    public IReadOnlyCollection<ShipmentReturnItemEntity> ShipmentReturnItems { get; private set; } = [];

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

	public void Update(
		string? name = null,
		ProductStatus? status = null,
		decimal? planPerHour = null)
	{
		if (name is not null)
		{
			Guard.AgainstNullOrWhiteSpace(name, "Product name cannot be empty.");
			Name = name;
		}
		if (status is not null)
		{
			Guard.AgainstNull(status, "Status cannot be null.");
			Status = status.Value;
		}
		if (planPerHour is not null)
		{
			Guard.AgainstNonPositive(planPerHour.Value, "PlanPerHour must be positive if specified.");
			PlanPerHour = planPerHour;
		}
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

	public void UpdateQty(decimal qtyPerUnit)
	{
		Guard.AgainstNegative(qtyPerUnit, "QtyPerUnit must be positive.");
		QtyPerUnit = qtyPerUnit;
    }
}

public class ProductDepartmentCostEntity : ActivatableEntity
{
	public Guid ProductId { get; private set; }
	public Guid DepartmentId { get; private set; }
	public decimal ExpensesPerUnit { get; private set; }
	public decimal CutCostPerUnit { get; private set; }
	public decimal SewingCostPerUnit { get; private set; }
	public decimal PackCostPerUnit { get; private set; }

	// Navigation properties
	public ProductEntity? Product { get; private set; }
	public DepartmentEntity? Department { get; private set; }

	public ProductDepartmentCostEntity(
		Guid productId,
		Guid departmentId,
		decimal expensesPerUnit,
		decimal cutCostPerUnit,
		decimal sewingCostPerUnit,
		decimal packCostPerUnit)
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
	}

	public void Update(
		decimal? expenses = null,
		decimal? cut = null,
		decimal? sewing = null,
		decimal? pack = null)
	{
		if (expenses is not null)
		{
			Guard.AgainstNegative(expenses.Value, "ExpensesPerUnit cannot be negative.");
			ExpensesPerUnit = expenses.Value;
		}
		if (cut is not null)
		{
			Guard.AgainstNegative(cut.Value, "CutCostPerUnit cannot be negative.");
			CutCostPerUnit = cut.Value;
		}
		if (sewing is not null)
		{
			Guard.AgainstNegative(sewing.Value, "SewingCostPerUnit cannot be negative.");
			SewingCostPerUnit = sewing.Value;
		}
		if (pack is not null)
		{
			Guard.AgainstNegative(pack.Value, "PackCostPerUnit cannot be negative.");
			PackCostPerUnit = pack.Value;
		}
    }
}