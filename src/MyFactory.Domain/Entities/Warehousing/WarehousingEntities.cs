using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Domain.Entities.Warehousing;

/// <summary>
/// Aggregate root representing a physical warehouse that stores raw materials.
/// </summary>
public sealed class Warehouse : BaseEntity
{
    private readonly List<InventoryItem> _inventoryItems = new();

    private Warehouse()
    {
    }

    public Warehouse(string name, string type, string location)
    {
        Rename(name);
        ChangeType(type);
        ChangeLocation(location);
    }

    public string Name { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public IReadOnlyCollection<InventoryItem> InventoryItems => _inventoryItems.AsReadOnly();

    public void Rename(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Name = name.Trim();
    }

    public void ChangeType(string type)
    {
        Guard.AgainstNullOrWhiteSpace(type, nameof(type));
        Type = type.Trim();
    }

    public void ChangeLocation(string location)
    {
        Guard.AgainstNullOrWhiteSpace(location, nameof(location));
        Location = location.Trim();
    }

    public InventoryItem AddInventory(Guid materialId)
    {
        Guard.AgainstEmptyGuid(materialId, nameof(materialId));
        if (_inventoryItems.Any(item => item.MaterialId == materialId))
        {
            throw new DomainException("Inventory already exists for the specified material.");
        }

        var inventoryItem = new InventoryItem(Id, materialId);
        _inventoryItems.Add(inventoryItem);
        return inventoryItem;
    }
}

/// <summary>
/// Inventory item that tracks stock for a single material within a warehouse.
/// </summary>
public sealed class InventoryItem : BaseEntity
{
    private readonly List<InventoryReceiptItem> _receiptItems = new();

    private InventoryItem()
    {
    }

    public InventoryItem(Guid warehouseId, Guid materialId)
    {
        Guard.AgainstEmptyGuid(warehouseId, nameof(warehouseId));
        Guard.AgainstEmptyGuid(materialId, nameof(materialId));

        WarehouseId = warehouseId;
        MaterialId = materialId;
    }

    public Guid WarehouseId { get; private set; }
    public Warehouse? Warehouse { get; private set; }
    public Guid MaterialId { get; private set; }
    public Material? Material { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal AveragePrice { get; private set; }
    public decimal ReservedQuantity { get; private set; }
    public decimal AvailableQuantity => Quantity - ReservedQuantity;
    public IReadOnlyCollection<InventoryReceiptItem> ReceiptItems => _receiptItems.AsReadOnly();

    public void Receive(decimal quantity, decimal unitPrice)
    {
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Guard.AgainstNonPositive(unitPrice, nameof(unitPrice));

        var newQuantity = Quantity + quantity;
        var totalValue = (Quantity * AveragePrice) + (quantity * unitPrice);
        AveragePrice = totalValue / newQuantity;
        Quantity = newQuantity;
    }

    public void Issue(decimal quantity)
    {
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        if (quantity > AvailableQuantity)
        {
            throw new DomainException("Cannot issue more than available quantity.");
        }

        Quantity -= quantity;
    }

    public void Reserve(decimal quantity)
    {
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        if (quantity > AvailableQuantity)
        {
            throw new DomainException("Cannot reserve more than available stock.");
        }

        ReservedQuantity += quantity;
    }

    public void ReleaseReservation(decimal quantity)
    {
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        if (quantity > ReservedQuantity)
        {
            throw new DomainException("Cannot release more than reserved quantity.");
        }

        ReservedQuantity -= quantity;
    }

    internal void RegisterReceiptItem(InventoryReceiptItem item)
    {
        Guard.AgainstNull(item, nameof(item));
        if (item.MaterialId != MaterialId)
        {
            throw new DomainException("Receipt item material mismatch.");
        }

        if (_receiptItems.Any(existing => existing.Id == item.Id))
        {
            return;
        }

        Receive(item.Quantity, item.UnitPrice);
        _receiptItems.Add(item);
    }
}

/// <summary>
/// Aggregate root for supplier receipts that bring materials into inventory.
/// </summary>
public sealed class InventoryReceipt : BaseEntity
{
    private readonly List<InventoryReceiptItem> _items = new();

    private InventoryReceipt()
    {
    }

    public InventoryReceipt(string receiptNumber, Guid supplierId, DateOnly receiptDate)
    {
        Guard.AgainstNullOrWhiteSpace(receiptNumber, nameof(receiptNumber));
        Guard.AgainstEmptyGuid(supplierId, nameof(supplierId));
        Guard.AgainstDefaultDate(receiptDate, nameof(receiptDate));

        ReceiptNumber = receiptNumber.Trim();
        SupplierId = supplierId;
        ReceiptDate = receiptDate;
        Status = InventoryReceiptStatuses.Draft;
    }

    public string ReceiptNumber { get; private set; } = string.Empty;
    public Guid SupplierId { get; private set; }
    public Supplier? Supplier { get; private set; }
    public DateOnly ReceiptDate { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string Status { get; private set; } = InventoryReceiptStatuses.Draft;
    public IReadOnlyCollection<InventoryReceiptItem> Items => _items.AsReadOnly();

    public InventoryReceiptItem AddItem(Guid materialId, decimal quantity, decimal unitPrice, Guid? inventoryItemId = null)
    {
        EnsureDraft();

        if (_items.Any(entry => entry.MaterialId == materialId))
        {
            throw new DomainException("Material already exists within this receipt.");
        }

        var item = new InventoryReceiptItem(Id, materialId, quantity, unitPrice, inventoryItemId);
        _items.Add(item);
        TotalAmount += item.LineTotal;
        return item;
    }

    public InventoryReceiptItem AddItem(Guid materialId, decimal quantity, decimal unitPrice, InventoryItem inventoryItem)
    {
        Guard.AgainstNull(inventoryItem, nameof(inventoryItem));
        if (inventoryItem.MaterialId != materialId)
        {
            throw new DomainException("Inventory item material mismatch.");
        }

        var item = AddItem(materialId, quantity, unitPrice, inventoryItem.Id);
        item.AssignInventoryItem(inventoryItem);
        return item;
    }

    public void MarkAsReceived()
    {
        EnsureDraft();
        if (_items.Count == 0)
        {
            throw new DomainException("Cannot mark receipt without items as received.");
        }

        Status = InventoryReceiptStatuses.Received;
    }

    public void Cancel()
    {
        EnsureDraft();
        Status = InventoryReceiptStatuses.Cancelled;
    }

    private void EnsureDraft()
    {
        if (Status != InventoryReceiptStatuses.Draft)
        {
            throw new DomainException("Only draft receipts can be modified.");
        }
    }

    internal void LinkSupplier(Supplier supplier)
    {
        Guard.AgainstNull(supplier, nameof(supplier));
        if (supplier.Id != SupplierId)
        {
            throw new DomainException("Supplier reference mismatch for inventory receipt.");
        }

        Supplier = supplier;
    }
}

/// <summary>
/// Line item within an inventory receipt.
/// </summary>
public sealed class InventoryReceiptItem : BaseEntity
{
    private InventoryReceiptItem()
    {
    }

    public InventoryReceiptItem(Guid receiptId, Guid materialId, decimal quantity, decimal unitPrice, Guid? inventoryItemId = null)
    {
        Guard.AgainstEmptyGuid(receiptId, nameof(receiptId));
        Guard.AgainstEmptyGuid(materialId, nameof(materialId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Guard.AgainstNonPositive(unitPrice, nameof(unitPrice));

        InventoryReceiptId = receiptId;
        MaterialId = materialId;
        Quantity = quantity;
        UnitPrice = unitPrice;

        if (inventoryItemId.HasValue)
        {
            AssignInventoryItem(inventoryItemId.Value);
        }
    }

    public Guid InventoryReceiptId { get; private set; }
    public InventoryReceipt? InventoryReceipt { get; private set; }
    public Guid MaterialId { get; private set; }
    public Material? Material { get; private set; }
    public Guid? InventoryItemId { get; private set; }
    public InventoryItem? InventoryItem { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal LineTotal => Quantity * UnitPrice;

    public void AssignInventoryItem(Guid inventoryItemId)
    {
        Guard.AgainstEmptyGuid(inventoryItemId, nameof(inventoryItemId));
        InventoryItemId = inventoryItemId;
        InventoryItem = null;
    }

    public void AssignInventoryItem(InventoryItem inventoryItem)
    {
        Guard.AgainstNull(inventoryItem, nameof(inventoryItem));
        if (inventoryItem.MaterialId != MaterialId)
        {
            throw new DomainException("Inventory item material mismatch.");
        }

        InventoryItemId = inventoryItem.Id;
        InventoryItem = inventoryItem;
        inventoryItem.RegisterReceiptItem(this);
    }
}

public static class InventoryReceiptStatuses
{
    public const string Draft = "Draft";
    public const string Received = "Received";
    public const string Cancelled = "Cancelled";
}

/// <summary>
/// Aggregate root for purchase requests that trigger procurement of materials.
/// </summary>
public sealed class PurchaseRequest : BaseEntity
{
    private readonly List<PurchaseRequestItem> _items = new();

    private PurchaseRequest()
    {
    }

    public PurchaseRequest(string prNumber, DateOnly createdAt)
    {
        Guard.AgainstNullOrWhiteSpace(prNumber, nameof(prNumber));
        Guard.AgainstDefaultDate(createdAt, nameof(createdAt));

        PrNumber = prNumber.Trim();
        CreatedAt = createdAt;
        Status = PurchaseRequestStatuses.Draft;
    }

    public string PrNumber { get; private set; } = string.Empty;
    public DateOnly CreatedAt { get; private set; }
    public string Status { get; private set; } = PurchaseRequestStatuses.Draft;
    public IReadOnlyCollection<PurchaseRequestItem> Items => _items.AsReadOnly();

    public PurchaseRequestItem AddItem(Guid materialId, decimal quantity)
    {
        EnsureDraft();
        Guard.AgainstEmptyGuid(materialId, nameof(materialId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));

        var existing = _items.FirstOrDefault(item => item.MaterialId == materialId);
        if (existing is not null)
        {
            existing.IncreaseQuantity(quantity);
            return existing;
        }

        var item = new PurchaseRequestItem(Id, materialId, quantity);
        _items.Add(item);
        return item;
    }

    public PurchaseRequestItem UpdateItem(Guid itemId, Guid materialId, decimal quantity)
    {
        EnsureDraft();
        Guard.AgainstEmptyGuid(itemId, nameof(itemId));
        Guard.AgainstEmptyGuid(materialId, nameof(materialId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));

        var item = _items.FirstOrDefault(entity => entity.Id == itemId)
            ?? throw new DomainException("Purchase request item not found.");

        if (_items.Any(entity => entity.Id != itemId && entity.MaterialId == materialId))
        {
            throw new DomainException("Material already exists within this purchase request.");
        }

        item.Update(materialId, quantity);
        return item;
    }

    public void RemoveItem(Guid itemId)
    {
        EnsureDraft();
        Guard.AgainstEmptyGuid(itemId, nameof(itemId));

        var item = _items.FirstOrDefault(entity => entity.Id == itemId)
            ?? throw new DomainException("Purchase request item not found.");

        _items.Remove(item);
    }

    public void Submit()
    {
        EnsureDraft();
        if (!_items.Any())
        {
            throw new DomainException("Cannot submit a purchase request without items.");
        }

        Status = PurchaseRequestStatuses.Submitted;
    }

    public void Approve()
    {
        if (Status != PurchaseRequestStatuses.Submitted)
        {
            throw new DomainException("Only submitted purchase requests can be approved.");
        }

        Status = PurchaseRequestStatuses.Approved;
    }

    public void Reject()
    {
        if (Status != PurchaseRequestStatuses.Submitted)
        {
            throw new DomainException("Only submitted purchase requests can be rejected.");
        }

        Status = PurchaseRequestStatuses.Rejected;
    }

    public void Cancel()
    {
        if (Status != PurchaseRequestStatuses.Draft && Status != PurchaseRequestStatuses.Submitted)
        {
            throw new DomainException("Only draft or submitted purchase requests can be cancelled.");
        }

        Status = PurchaseRequestStatuses.Cancelled;
    }

    private void EnsureDraft()
    {
        if (Status != PurchaseRequestStatuses.Draft)
        {
            throw new DomainException("Only draft purchase requests can be modified.");
        }
    }
}

/// <summary>
/// Line item within a purchase request.
/// </summary>
public sealed class PurchaseRequestItem : BaseEntity
{
    private PurchaseRequestItem()
    {
    }

    public PurchaseRequestItem(Guid purchaseRequestId, Guid materialId, decimal quantity)
    {
        Guard.AgainstEmptyGuid(purchaseRequestId, nameof(purchaseRequestId));
        Guard.AgainstEmptyGuid(materialId, nameof(materialId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));

        PurchaseRequestId = purchaseRequestId;
        MaterialId = materialId;
        Quantity = quantity;
    }

    public Guid PurchaseRequestId { get; }
    public PurchaseRequest? PurchaseRequest { get; private set; }
    public Guid MaterialId { get; private set; }
    public Material? Material { get; private set; }
    public decimal Quantity { get; private set; }

    public void IncreaseQuantity(decimal quantity)
    {
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Quantity += quantity;
    }

    public void Update(Guid materialId, decimal quantity)
    {
        Guard.AgainstEmptyGuid(materialId, nameof(materialId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));

        MaterialId = materialId;
        Quantity = quantity;
    }
}

public static class PurchaseRequestStatuses
{
    public const string Draft = "Draft";
    public const string Submitted = "Submitted";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
    public const string Cancelled = "Cancelled";
}
