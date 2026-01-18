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
	public int? PlanPerHour { get; private set; }
    public Guid? PayrollRuleId { get; private set; }

	// Navigation properties
	public ICollection<ProductDepartmentCostEntity> ProductDepartmentCosts { get; private set; } = [];
	public ICollection<ProductMaterialEntity> ProductMaterials { get; private set; } = [];
    public ICollection<FinishedGoodsEntity> FinishedGoods { get; private set; } = [];
    public ICollection<WarehouseProductEntity> WarehouseProducts { get; private set; } = [];
    public ICollection<FinishedGoodsMovementItemEntity> FinishedGoodsMovementItems { get; private set; } = [];
    public ICollection<ShipmentItemEntity> ShipmentItems { get; private set; } = [];
    public ICollection<FinishedGoodsStockEntity> FinishedGoodsStocks { get; private set; } = [];
    public ICollection<ShipmentReturnItemEntity> ShipmentReturnItems { get; private set; } = [];
    public ICollection<ProductImageEntity> ProductImages { get; private set; } = [];

    public ProductEntity(
		string sku,
		string name,
		ProductStatus status,
		int? version = null,
		string? description = null,
		int? planPerHour = null,
        Guid? payrollRuleId = null)
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
        PayrollRuleId = payrollRuleId;
	}

	public void Update(
		string? name = null,
		ProductStatus? status = null,
		int? planPerHour = null,
		string? description = null,
		int? version = null,
        Guid? payrollRuleId = null)
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
		if (description is not null)
		{
			Description = description;
		}
		if (version is not null)
		{
			Guard.AgainstNonPositive(version.Value, "Version must be positive if specified.");
			Version = version;
		}
        if (payrollRuleId is not null)
        {
            PayrollRuleId = payrollRuleId;
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

public class ProductImageEntity : AuditableEntity
{
    public Guid ProductId { get; private set; }
    public string FileName { get; private set; }
    public string Path { get; private set; }
    public string? ContentType { get; private set; }
    public int SortOrder { get; private set; }

    // Navigation properties
    public ProductEntity? Product { get; private set; }

    public ProductImageEntity(Guid productId, string fileName, string path, string? contentType = null, int sortOrder = 0)
    {
        Guard.AgainstEmptyGuid(productId, "ProductId is required.");
        Guard.AgainstNullOrWhiteSpace(fileName, "FileName is required.");
        Guard.AgainstNullOrWhiteSpace(path, "Path is required.");

        ProductId = productId;
        FileName = fileName;
        Path = path;
        ContentType = contentType;
        SortOrder = sortOrder;
    }

    public void UpdateFile(string fileName, string path, string? contentType = null)
    {
        Guard.AgainstNullOrWhiteSpace(fileName, "FileName is required.");
        Guard.AgainstNullOrWhiteSpace(path, "Path is required.");

        FileName = fileName;
        Path = path;
        ContentType = contentType;
        Touch();
    }

    public void SetSortOrder(int sortOrder)
    {
        SortOrder = sortOrder;
        Touch();
    }
}