using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Products;
using MyFactory.Domain.Exceptions;

namespace MyFactory.Domain.Entities.Materials;

public class MaterialEntity : ActivatableEntity
{
	public string Name { get; private set; }
	public Guid MaterialTypeId { get; private set; }
	public Guid UnitId { get; private set; }
	public string? Color { get; private set; }

	// Navigation properties
	public MaterialTypeEntity? MaterialType { get; private set; }
	public UnitEntity? Unit { get; private set; }
	public IReadOnlyCollection<ProductMaterialEntity> ProductMaterials { get; private set; } = new List<ProductMaterialEntity>();
	public IReadOnlyCollection<WarehouseMaterialEntity> WarehouseMaterials { get; private set; } = new List<WarehouseMaterialEntity>();
	public IReadOnlyCollection<MaterialSupplierEntity> MaterialSuppliers { get; private set; } = new List<MaterialSupplierEntity>();
	public IReadOnlyCollection<InventoryMovementItemEntity> InventoryMovementItems { get; private set; } = new List<InventoryMovementItemEntity>();
	public IReadOnlyCollection<MaterialPurchaseOrderItemEntity> MaterialPurchaseOrderItems { get; private set; } = new List<MaterialPurchaseOrderItemEntity>();

    public MaterialEntity(string name, Guid materialTypeId, Guid unitId, string? color = null)
	{
		Guard.AgainstNullOrWhiteSpace(name, "Material name is required.");
		Guard.AgainstEmptyGuid(materialTypeId, "MaterialTypeId is required.");
		Guard.AgainstEmptyGuid(unitId, "UnitId is required.");

		Name = name;
		MaterialTypeId = materialTypeId;
		UnitId = unitId;
		Color = color;
	}

    public void Update(string name, Guid materialTypeId, Guid unitId, string? color)
	{
		Name = name;
		MaterialTypeId = materialTypeId;
		UnitId = unitId;
		Color = color;
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

	public void Update(string name, string? description)
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

	public void Update(string code, string name)
	{
		Guard.AgainstNullOrWhiteSpace(code, "Unit code is required.");
		Guard.AgainstNullOrWhiteSpace(name, "Unit name is required.");
		Code = code;
		Name = name;
	}
}

public class SupplierEntity : ActivatableEntity
{
	public string Name { get; private set; }
	public string? Description { get; private set; }

	// Navigation properties
	public IReadOnlyCollection<MaterialSupplierEntity> MaterialSuppliers { get; private set; } = new List<MaterialSupplierEntity>();
	public IReadOnlyCollection<MaterialPurchaseOrderEntity> MaterialPurchaseOrders { get; private set; } = new List<MaterialPurchaseOrderEntity>();

    public SupplierEntity(string name, string? description = null)
	{
		Guard.AgainstNullOrWhiteSpace(name, "Supplier name is required.");
		Name = name;
		Description = description;
		UpdatedAt = DateTime.UtcNow;
	}

	public void Update(string name, string? description)
	{
		Name = name;
		Description = description;

	}
}

public class MaterialSupplierEntity : ActivatableEntity
{
	public Guid MaterialId { get; private set; }
	public Guid SupplierId { get; private set; }
	public decimal? MinOrderQty { get; private set; }

	// Navigation properties
	public MaterialEntity? Material { get; private set; }
	public SupplierEntity? Supplier { get; private set; }

	public MaterialSupplierEntity(Guid materialId, Guid supplierId, decimal? minOrderQty = null)
	{
		Guard.AgainstEmptyGuid(materialId, "MaterialId is required.");
		Guard.AgainstEmptyGuid(supplierId, "SupplierId is required.");
		if (minOrderQty.HasValue)
			Guard.AgainstNegative(minOrderQty.Value, "MinOrderQty cannot be negative.");
		MaterialId = materialId;
		SupplierId = supplierId;
		MinOrderQty = minOrderQty;
	}
}

public class MaterialPurchaseOrderEntity : AuditableEntity
{
    public Guid SupplierId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public PurchaseOrderStatus Status { get; private set; }

    // Navigation properties
	public SupplierEntity? Supplier { get; private set; }
    public IReadOnlyCollection<MaterialPurchaseOrderItemEntity> MaterialPurchaseItems { get; private set; } = new List<MaterialPurchaseOrderItemEntity>();

    public MaterialPurchaseOrderEntity(Guid supplierId, DateTime orderDate)
    {
        Guard.AgainstEmptyGuid(supplierId, nameof(supplierId));
        Guard.AgainstDefaultDate(orderDate, nameof(orderDate));
        SupplierId = supplierId;
        OrderDate = orderDate;
        Status = PurchaseOrderStatus.New;
    }

    public void Confirm()
    {
        if (Status != PurchaseOrderStatus.New)
            throw new DomainException("Only new orders can be confirmed.");
        Status = PurchaseOrderStatus.Confirmed;
		Touch();
    }

    public void Receive()
    {
        if (Status != PurchaseOrderStatus.Confirmed)
            throw new DomainException("Only confirmed orders can be received.");
        Status = PurchaseOrderStatus.Received;
        Touch();
    }

    public void Cancel()
    {
        if (Status == PurchaseOrderStatus.Received || Status == PurchaseOrderStatus.Cancelled)
            throw new DomainException("Cannot cancel a received or already cancelled order.");
        Status = PurchaseOrderStatus.Cancelled;
        Touch();
    }

	public void EnsureEditable()
	{
		if (Status != PurchaseOrderStatus.New)
			throw new DomainException("Only new orders can be modified.");
    }

	public static MaterialPurchaseOrderEntity Create(Guid supplierId, DateTime orderDate)
	{
		return new MaterialPurchaseOrderEntity(supplierId, orderDate);
	}
}

public enum PurchaseOrderStatus
{
    New,
    Confirmed,
    Received,
    Cancelled
}

public class MaterialPurchaseOrderItemEntity : AuditableEntity
{
    public Guid PurchaseOrderId { get; private set; }
    public Guid MaterialId { get; private set; }
    public decimal Qty { get; private set; }
    public decimal UnitPrice { get; private set; }
    public string MaterialName { get; private set; }
    public string UnitCode { get; private set; }

    // Navigation properties
	public MaterialEntity? Material { get; private set; }
	public MaterialPurchaseOrderEntity? MaterialPurchaseOrder { get; private set; }


    public MaterialPurchaseOrderItemEntity(Guid purchaseOrderId, Guid materialId, decimal qty, decimal unitPrice, string materialName, string unitCode)
    {
        Guard.AgainstEmptyGuid(purchaseOrderId, nameof(purchaseOrderId));
        Guard.AgainstEmptyGuid(materialId, nameof(materialId));
        Guard.AgainstNonPositive(qty, nameof(qty));
        Guard.AgainstNegative(unitPrice, nameof(unitPrice));
        Guard.AgainstNullOrWhiteSpace(materialName, nameof(materialName));
        Guard.AgainstNullOrWhiteSpace(unitCode, nameof(unitCode));

        PurchaseOrderId = purchaseOrderId;
        MaterialId = materialId;
        Qty = qty;
        UnitPrice = unitPrice;
		MaterialName = materialName;
		UnitCode = unitCode;
    }

	public void UpdateQty(decimal qty, MaterialPurchaseOrderEntity order)
	{
		order.EnsureEditable();
		Guard.AgainstNonPositive(qty, nameof(qty));
		Qty = qty;
		Touch();
    }

    public void UpdateUnitPrice(decimal unitPrice, MaterialPurchaseOrderEntity order)
    {
        order.EnsureEditable();
        Guard.AgainstNegative(unitPrice, nameof(unitPrice));
        UnitPrice = unitPrice;
        Touch();
    }
}