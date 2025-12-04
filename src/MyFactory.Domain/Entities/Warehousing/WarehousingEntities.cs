using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Domain.Entities.Warehousing;

public sealed class InventoryItem : BaseEntity
{
    private InventoryItem()
    {
    }

    public InventoryItem(Guid materialId, decimal quantityOnHand, decimal reservedQuantity, string location)
    {
        Guard.AgainstEmptyGuid(materialId, nameof(materialId));
        Guard.AgainstNegative(quantityOnHand, nameof(quantityOnHand));
        Guard.AgainstNegative(reservedQuantity, nameof(reservedQuantity));
        Guard.AgainstNullOrWhiteSpace(location, nameof(location));

        MaterialId = materialId;
        QuantityOnHand = quantityOnHand;
        ReservedQuantity = reservedQuantity;
        Location = location.Trim();
    }

    public Guid MaterialId { get; }
    public Material? Material { get; private set; }
    public decimal QuantityOnHand { get; private set; }
    public decimal ReservedQuantity { get; private set; }
    public string Location { get; private set; } = string.Empty;

    public decimal AvailableQuantity => QuantityOnHand - ReservedQuantity;

    public void AdjustOnHand(decimal delta)
    {
        var newQuantity = QuantityOnHand + delta;
        if (newQuantity < 0)
        {
            throw new DomainException("Resulting stock cannot be negative.");
        }

        QuantityOnHand = newQuantity;
    }

    public void AdjustReserved(decimal delta)
    {
        var newReserved = ReservedQuantity + delta;
        if (newReserved < 0 || newReserved > QuantityOnHand)
        {
            throw new DomainException("Reserved quantity must be between 0 and on-hand.");
        }

        ReservedQuantity = newReserved;
    }
}

public sealed class InventoryReceipt : BaseEntity
{
    private readonly List<InventoryReceiptItem> _items = new();

    private InventoryReceipt()
    {
    }

    public InventoryReceipt(Guid supplierId, DateTime receiptDate, string documentNumber)
    {
        Guard.AgainstEmptyGuid(supplierId, nameof(supplierId));
        Guard.AgainstDefaultDate(receiptDate, nameof(receiptDate));
        Guard.AgainstNullOrWhiteSpace(documentNumber, nameof(documentNumber));

        SupplierId = supplierId;
        ReceiptDate = receiptDate;
        DocumentNumber = documentNumber.Trim();
    }

    public Guid SupplierId { get; }
    public Supplier? Supplier { get; private set; }
    public DateTime ReceiptDate { get; private set; }
    public string DocumentNumber { get; private set; } = string.Empty;
    public IReadOnlyCollection<InventoryReceiptItem> Items => _items.AsReadOnly();

    public void AddItem(Guid materialId, decimal quantity, decimal unitCost)
    {
        var item = new InventoryReceiptItem(Id, materialId, quantity, unitCost);
        _items.Add(item);
    }
}

public sealed class InventoryReceiptItem : BaseEntity
{
    private InventoryReceiptItem()
    {
    }

    public InventoryReceiptItem(Guid receiptId, Guid materialId, decimal quantity, decimal unitCost)
    {
        Guard.AgainstEmptyGuid(receiptId, nameof(receiptId));
        Guard.AgainstEmptyGuid(materialId, nameof(materialId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Guard.AgainstNonPositive(unitCost, nameof(unitCost));

        InventoryReceiptId = receiptId;
        MaterialId = materialId;
        Quantity = quantity;
        UnitCost = unitCost;
    }

    public Guid InventoryReceiptId { get; }
    public InventoryReceipt? InventoryReceipt { get; private set; }
    public Guid MaterialId { get; }
    public Material? Material { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal UnitCost { get; private set; }
    public decimal TotalCost => Quantity * UnitCost;
}

public sealed class PurchaseRequest : BaseEntity
{
    private readonly List<PurchaseRequestItem> _items = new();

    private PurchaseRequest()
    {
    }

    public PurchaseRequest(Guid requestedByEmployeeId, DateTime requestedOn, string status)
    {
        Guard.AgainstEmptyGuid(requestedByEmployeeId, nameof(requestedByEmployeeId));
        Guard.AgainstDefaultDate(requestedOn, nameof(requestedOn));
        Guard.AgainstNullOrWhiteSpace(status, nameof(status));

        RequestedByEmployeeId = requestedByEmployeeId;
        RequestedOn = requestedOn;
        Status = status.Trim();
    }

    public Guid RequestedByEmployeeId { get; }
    public DateTime RequestedOn { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public IReadOnlyCollection<PurchaseRequestItem> Items => _items.AsReadOnly();

    public void ChangeStatus(string status)
    {
        Guard.AgainstNullOrWhiteSpace(status, nameof(status));
        Status = status.Trim();
    }

    public void AddItem(Guid materialId, decimal quantity, string unit)
    {
        var item = new PurchaseRequestItem(Id, materialId, quantity, unit);
        _items.Add(item);
    }
}

public sealed class PurchaseRequestItem : BaseEntity
{
    private PurchaseRequestItem()
    {
    }

    public PurchaseRequestItem(Guid purchaseRequestId, Guid materialId, decimal quantity, string unit)
    {
        Guard.AgainstEmptyGuid(purchaseRequestId, nameof(purchaseRequestId));
        Guard.AgainstEmptyGuid(materialId, nameof(materialId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Guard.AgainstNullOrWhiteSpace(unit, nameof(unit));

        PurchaseRequestId = purchaseRequestId;
        MaterialId = materialId;
        Quantity = quantity;
        Unit = unit.Trim();
    }

    public Guid PurchaseRequestId { get; }
    public PurchaseRequest? PurchaseRequest { get; private set; }
    public Guid MaterialId { get; }
    public Material? Material { get; private set; }
    public decimal Quantity { get; private set; }
    public string Unit { get; private set; } = string.Empty;
}
