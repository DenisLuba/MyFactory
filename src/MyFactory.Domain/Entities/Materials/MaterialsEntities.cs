using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Products;

namespace MyFactory.Domain.Entities.Materials;

public class MaterialEntity : AuditableEntity
{
	public string Name { get; private set; }
	public Guid MaterialTypeId { get; private set; }
	public Guid UnitId { get; private set; }
	public string? Color { get; private set; }
	public bool IsActive { get; private set; }

	// Navigation properties
	public MaterialTypeEntity? MaterialType { get; private set; }
	public UnitEntity? Unit { get; private set; }
	public IReadOnlyCollection<ProductMaterialEntity> ProductMaterials { get; private set; } = new List<ProductMaterialEntity>();
	public IReadOnlyCollection<WarehouseMaterialEntity> WarehouseMaterials { get; private set; } = new List<WarehouseMaterialEntity>();
	public IReadOnlyCollection<MaterialSupplierEntity> MaterialSuppliers { get; private set; } = new List<MaterialSupplierEntity>();
	public IReadOnlyCollection<MaterialSupplierPriceEntity> MaterialSupplierPrices { get; private set; } = new List<MaterialSupplierPriceEntity>();
	public IReadOnlyCollection<InventoryMovementItemEntity> InventoryMovementItems { get; private set; } = new List<InventoryMovementItemEntity>();

	public MaterialEntity(string name, Guid materialTypeId, Guid unitId, string? color = null, bool isActive = true)
	{
		Guard.AgainstNullOrWhiteSpace(name, "Material name is required.");
		Guard.AgainstEmptyGuid(materialTypeId, "MaterialTypeId is required.");
		Guard.AgainstEmptyGuid(unitId, "UnitId is required.");
		Name = name;
		MaterialTypeId = materialTypeId;
		UnitId = unitId;
		Color = color;
		IsActive = isActive;
	}
}

public class MaterialTypeEntity : BaseEntity
{
	public string Name { get; private set; }
	public string? Description { get; private set; }

	// Navigation properties
	public IReadOnlyCollection<MaterialEntity> Materials { get; private set; } = new List<MaterialEntity>();

	public MaterialTypeEntity(string name, string? description = null)
	{
		Guard.AgainstNullOrWhiteSpace(name, "Material type name is required.");
		Name = name;
		Description = description;
	}
}

public class UnitEntity : BaseEntity
{
	public string Code { get; private set; }
	public string Name { get; private set; }

	// Navigation properties
	public IReadOnlyCollection<MaterialEntity> Materials { get; private set; } = new List<MaterialEntity>();

	public UnitEntity(string code, string name)
	{
		Guard.AgainstNullOrWhiteSpace(code, "Unit code is required.");
		Guard.AgainstNullOrWhiteSpace(name, "Unit name is required.");
		Code = code;
		Name = name;
	}
}

public class SupplierEntity : AuditableEntity
{
	public string Name { get; private set; }
	public bool IsActive { get; private set; }

	// Navigation properties
	public IReadOnlyCollection<MaterialSupplierEntity> MaterialSuppliers { get; private set; } = new List<MaterialSupplierEntity>();
	public IReadOnlyCollection<MaterialSupplierPriceEntity> MaterialSupplierPrices { get; private set; } = new List<MaterialSupplierPriceEntity>();

	public SupplierEntity(string name, bool isActive = true)
	{
		Guard.AgainstNullOrWhiteSpace(name, "Supplier name is required.");
		Name = name;
		IsActive = isActive;
	}
}

public class MaterialSupplierEntity : AuditableEntity
{
	public Guid MaterialId { get; private set; }
	public Guid SupplierId { get; private set; }
	public decimal? MinOrderQty { get; private set; }
	public bool IsActive { get; private set; }

	// Navigation properties
	public MaterialEntity? Material { get; private set; }
	public SupplierEntity? Supplier { get; private set; }

	public MaterialSupplierEntity(Guid materialId, Guid supplierId, decimal? minOrderQty = null, bool isActive = true)
	{
		Guard.AgainstEmptyGuid(materialId, "MaterialId is required.");
		Guard.AgainstEmptyGuid(supplierId, "SupplierId is required.");
		if (minOrderQty.HasValue)
			Guard.AgainstNegative(minOrderQty.Value, "MinOrderQty cannot be negative.");
		MaterialId = materialId;
		SupplierId = supplierId;
		MinOrderQty = minOrderQty;
		IsActive = isActive;
	}
}

public class MaterialSupplierPriceEntity : BaseEntity
{
	public Guid SupplierId { get; private set; }
	public Guid MaterialId { get; private set; }
	public decimal PricePerUnit { get; private set; }

	// Navigation properties
	public SupplierEntity? Supplier { get; private set; }
	public MaterialEntity? Material { get; private set; }

	public MaterialSupplierPriceEntity(Guid supplierId, Guid materialId, decimal pricePerUnit)
	{
		Guard.AgainstEmptyGuid(supplierId, "SupplierId is required.");
		Guard.AgainstEmptyGuid(materialId, "MaterialId is required.");
		Guard.AgainstNonPositive(pricePerUnit, "PricePerUnit must be positive.");
		SupplierId = supplierId;
		MaterialId = materialId;
		PricePerUnit = pricePerUnit;
	}
}