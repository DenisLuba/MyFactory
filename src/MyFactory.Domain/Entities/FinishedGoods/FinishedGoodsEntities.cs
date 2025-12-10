using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Domain.Entities.FinishedGoods;

// Domain events
public sealed class ShipmentShipped
{
    public Guid ShipmentId { get; }
    public DateTime ShippedAt { get; }
    public ShipmentShipped(Guid shipmentId, DateTime shippedAt)
    {
        ShipmentId = shipmentId;
        ShippedAt = shippedAt;
    }
}

public sealed class ReturnReceived
{
    public Guid ReturnId { get; }
    public DateTime ReceivedAt { get; }
    public ReturnReceived(Guid returnId, DateTime receivedAt)
    {
        ReturnId = returnId;
        ReceivedAt = receivedAt;
    }
}

public sealed class FinishedGoodsInventory : BaseEntity
{
    private readonly List<FinishedGoodsMovement> _movements = new();

    private FinishedGoodsInventory()
    {
    }

    private FinishedGoodsInventory(Guid specificationId, Guid warehouseId, DateOnly? updatedAt = null)
    {
        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstEmptyGuid(warehouseId, nameof(warehouseId));

        SpecificationId = specificationId;
        WarehouseId = warehouseId;
        Quantity = 0m;
        UnitCost = 0m;
        UpdatedAt = updatedAt ?? DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public static FinishedGoodsInventory Create(Guid specificationId, Guid warehouseId, DateOnly? updatedAt = null)
        => new FinishedGoodsInventory(specificationId, warehouseId, updatedAt);

    public Guid SpecificationId { get; private set; }
    public Specification? Specification { get; internal set; }
    public Guid WarehouseId { get; private set; }
    public Warehouse? Warehouse { get; internal set; }
    public decimal Quantity { get; private set; }
    public decimal UnitCost { get; private set; }
    public DateOnly UpdatedAt { get; private set; }

    public IReadOnlyCollection<FinishedGoodsMovement> Movements => _movements.AsReadOnly();

    // TODO: Configure decimal precision/scale (e.g. decimal(18,4)) in Infrastructure mapping.
    // TODO: Ensure DateOnly mapping is configured in Infrastructure.

    public void Receive(decimal quantity, decimal unitCost, DateOnly receivedAt)
    {
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Guard.AgainstNegative(unitCost, nameof(unitCost));
        Guard.AgainstDefaultDate(receivedAt, nameof(receivedAt));

        var totalValue = (Quantity * UnitCost) + (quantity * unitCost);
        var newQuantity = Quantity + quantity;
        UnitCost = newQuantity == 0 ? 0 : totalValue / newQuantity;
        Quantity = newQuantity;
        UpdatedAt = receivedAt;
    }

    public void Issue(decimal quantity, DateOnly issuedAt)
    {
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Guard.AgainstDefaultDate(issuedAt, nameof(issuedAt));
        if (quantity > Quantity)
        {
            throw new DomainException("Cannot issue more than on-hand quantity.");
        }

        Quantity -= quantity;
        UpdatedAt = issuedAt;
    }

    public void ApplyShipment(decimal quantity, DateOnly shippedAt)
    {
        // Convenience: same semantics as Issue
        Issue(quantity, shippedAt);
    }

    public void ApplyCustomerReturn(decimal quantity, decimal unitCost, DateOnly receivedAt)
    {
        // Convenience: same semantics as Receive
        Receive(quantity, unitCost, receivedAt);
    }

    internal void AttachMovement(FinishedGoodsMovement movement)
    {
        Guard.AgainstNull(movement, nameof(movement));

        if (movement.FinishedGoodsInventoryId.HasValue && movement.FinishedGoodsInventoryId.Value != Id)
        {
            throw new DomainException("Movement already linked to a different finished goods inventory.");
        }

        // Validate specification/warehouse if needed (skip here - repository should ensure consistency)
        // Link movement to this inventory and keep in-memory graph consistent
        movement.AttachSourceInventory(Id, Quantity);
        movement.FinishedGoodsInventory = this;

        if (_movements.Any(m => m.Id == movement.Id))
        {
            return;
        }

        _movements.Add(movement);
    }

    internal void DetachMovement(FinishedGoodsMovement movement)
    {
        Guard.AgainstNull(movement, nameof(movement));
        var idx = _movements.FindIndex(m => m.Id == movement.Id);
        if (idx == -1) return;
        _movements.RemoveAt(idx);
        // Keep movement's FinishedGoodsInventory navigation cleared for in-memory consistency.
        movement.FinishedGoodsInventory = null;
        // Note: FinishedGoodsInventoryId is left as-is; clearing FK is the repository responsibility if required.
    }
}

public sealed class FinishedGoodsMovement : BaseEntity
{
    private FinishedGoodsMovement()
    {
    }

    private FinishedGoodsMovement(Guid specificationId, Guid fromWarehouseId, Guid toWarehouseId, decimal quantity, DateOnly movedAt, Guid? finishedGoodsInventoryId = null)
    {
        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstEmptyGuid(fromWarehouseId, nameof(fromWarehouseId));
        Guard.AgainstEmptyGuid(toWarehouseId, nameof(toWarehouseId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Guard.AgainstDefaultDate(movedAt, nameof(movedAt));

        if (fromWarehouseId == toWarehouseId)
        {
            throw new DomainException("Source and destination warehouses must differ.");
        }

        SpecificationId = specificationId;
        FromWarehouseId = fromWarehouseId;
        ToWarehouseId = toWarehouseId;
        Quantity = quantity;
        MovedAt = movedAt;
        FinishedGoodsInventoryId = finishedGoodsInventoryId;
    }

    public static FinishedGoodsMovement CreateTransfer(Guid specificationId, Guid fromWarehouseId, Guid toWarehouseId, decimal quantity, DateOnly movedAt, Guid? finishedGoodsInventoryId = null, decimal? sourceAvailableQuantity = null)
    {
        if (sourceAvailableQuantity.HasValue && quantity > sourceAvailableQuantity.Value)
        {
            throw new DomainException("Movement quantity cannot exceed available source quantity.");
        }

        var m = new FinishedGoodsMovement(specificationId, fromWarehouseId, toWarehouseId, quantity, movedAt, finishedGoodsInventoryId);
        // TODO: Publish domain event FinishedGoodsMovementCreated if needed via AddDomainEvent
        return m;
    }

    public Guid SpecificationId { get; private set; }
    public Specification? Specification { get; internal set; }
    public Guid FromWarehouseId { get; private set; }
    public Warehouse? FromWarehouse { get; internal set; }
    public Guid ToWarehouseId { get; private set; }
    public Warehouse? ToWarehouse { get; internal set; }
    public decimal Quantity { get; private set; }
    public DateOnly MovedAt { get; private set; }
    public Guid? FinishedGoodsInventoryId { get; private set; }
    public FinishedGoodsInventory? FinishedGoodsInventory { get; internal set; }

    /// <summary>
    /// Link movement to an existing finished goods inventory record (optional).
    /// Validates that movement quantity does not exceed provided available quantity.
    /// Does not mutate inventory; application layer must perform inventory updates.
    /// </summary>
    public void AttachSourceInventory(Guid finishedGoodsInventoryId, decimal sourceAvailableQuantity)
    {
        if (FinishedGoodsInventoryId.HasValue && FinishedGoodsInventoryId.Value != finishedGoodsInventoryId)
        {
            throw new DomainException("Movement already linked to a different source inventory.");
        }

        Guard.AgainstEmptyGuid(finishedGoodsInventoryId, nameof(finishedGoodsInventoryId));
        if (Quantity > sourceAvailableQuantity)
        {
            throw new DomainException("Movement quantity cannot exceed available source quantity.");
        }

        FinishedGoodsInventoryId = finishedGoodsInventoryId;
    }

    // NOTE: movement does not change inventories directly. Application layer or domain service must perform source.Issue and dest.Receive in a single transaction.
}

public sealed class Customer : BaseEntity
{
    public const int NameMaxLength = 200;
    public const int ContactMaxLength = 500;

    private Customer()
    {
    }

    private Customer(string name, string contact)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Guard.AgainstNullOrWhiteSpace(contact, nameof(contact));
        var n = name.Trim();
        var c = contact.Trim();
        if (n.Length > NameMaxLength) throw new DomainException($"Customer name cannot exceed {NameMaxLength} characters.");
        if (c.Length > ContactMaxLength) throw new DomainException($"Customer contact cannot exceed {ContactMaxLength} characters.");

        Name = n;
        Contact = c;
    }

    public static Customer Create(string name, string contact) => new Customer(name, contact);

    public string Name { get; private set; } = string.Empty;
    public string Contact { get; private set; } = string.Empty;

    public void UpdateName(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        var n = name.Trim();
        if (n.Length > NameMaxLength) throw new DomainException($"Customer name cannot exceed {NameMaxLength} characters.");
        Name = n;
    }

    public void UpdateContact(string contact)
    {
        Guard.AgainstNullOrWhiteSpace(contact, nameof(contact));
        var c = contact.Trim();
        if (c.Length > ContactMaxLength) throw new DomainException($"Customer contact cannot exceed {ContactMaxLength} characters.");
        Contact = c;
    }
}

public sealed class Shipment : BaseEntity
{
    public const int ShipmentNumberMaxLength = 100;

    private readonly List<ShipmentItem> _items = new();

    private Shipment()
    {
    }

    private Shipment(string shipmentNumber, Guid customerId, DateOnly date)
    {
        Guard.AgainstNullOrWhiteSpace(shipmentNumber, nameof(shipmentNumber));
        var sn = shipmentNumber.Trim();
        if (sn.Length > ShipmentNumberMaxLength) throw new DomainException($"Shipment number cannot exceed {ShipmentNumberMaxLength} characters.");
        Guard.AgainstEmptyGuid(customerId, nameof(customerId));
        Guard.AgainstDefaultDate(date, nameof(date));

        ShipmentNumber = sn;
        CustomerId = customerId;
        Date = date;
        Status = ShipmentStatuses.Draft;
        TotalAmount = 0m;
    }

    public static Shipment Create(string shipmentNumber, Guid customerId, DateOnly date) => new Shipment(shipmentNumber, customerId, date);

    public string ShipmentNumber { get; private set; } = string.Empty;
    public Guid CustomerId { get; private set; }
    public Customer? Customer { get; internal set; }
    public DateOnly Date { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string Status { get; private set; } = ShipmentStatuses.Draft;
    public IReadOnlyCollection<ShipmentItem> Items => _items.AsReadOnly();

    public ShipmentItem AddItem(Guid specificationId, decimal qty, decimal unitPrice)
    {
        EnsureDraft();

        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstNonPositive(qty, nameof(qty));
        Guard.AgainstNonPositive(unitPrice, nameof(unitPrice));

        // Prevent duplicate specification items within same shipment to avoid ambiguity. If business allows duplicates, change logic.
        if (_items.Any(i => i.SpecificationId == specificationId))
        {
            throw new DomainException("Specification already added to shipment.");
        }

        var item = ShipmentItem.Create(Id, specificationId, qty, unitPrice);
        _items.Add(item);
        RecalculateTotal();
        return item;
    }

    public void RemoveItem(Guid itemId)
    {
        EnsureDraft();

        var idx = _items.FindIndex(i => i.Id == itemId);
        if (idx == -1) throw new DomainException("Shipment item not found.");
        _items.RemoveAt(idx);
        RecalculateTotal();
    }

    private void RecalculateTotal()
    {
        TotalAmount = _items.Sum(i => i.LineTotal);
    }

    public void MarkAsShipped()
    {
        if (Status != ShipmentStatuses.Draft) throw new DomainException("Only draft shipments can be marked as shipped.");
        if (!_items.Any()) throw new DomainException("Cannot ship an empty shipment.");

        Status = ShipmentStatuses.Shipped;
        // publish domain event so application layer can perform inventory reductions
        AddDomainEvent(new ShipmentShipped(Id, DateTime.UtcNow));
        // TODO: application layer must decrement FinishedGoodsInventory or create FinishedGoodsMovement for each item in a single transaction.
    }

    public void Cancel()
    {
        if (Status != ShipmentStatuses.Draft) throw new DomainException("Only draft shipments can be cancelled.");
        Status = ShipmentStatuses.Cancelled;
    }

    private void EnsureDraft()
    {
        if (Status != ShipmentStatuses.Draft)
        {
            throw new DomainException("Only draft shipments can be modified.");
        }
    }
}

public sealed class ShipmentItem : BaseEntity
{
    private ShipmentItem()
    {
    }

    private ShipmentItem(Guid shipmentId, Guid specificationId, decimal qty, decimal unitPrice)
    {
        Guard.AgainstEmptyGuid(shipmentId, nameof(shipmentId));
        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstNonPositive(qty, nameof(qty));
        Guard.AgainstNonPositive(unitPrice, nameof(unitPrice));

        ShipmentId = shipmentId;
        SpecificationId = specificationId;
        Qty = qty;
        UnitPrice = unitPrice;
    }

    public static ShipmentItem Create(Guid shipmentId, Guid specificationId, decimal qty, decimal unitPrice)
        => new ShipmentItem(shipmentId, specificationId, qty, unitPrice);

    public Guid ShipmentId { get; private set; }
    public Shipment? Shipment { get; internal set; }
    public Guid SpecificationId { get; private set; }
    public Specification? Specification { get; internal set; }
    public decimal Qty { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal LineTotal => Qty * UnitPrice;
}

public static class ShipmentStatuses
{
    public const string Draft = "Draft";
    public const string Shipped = "Shipped";
    public const string Cancelled = "Cancelled";
}

public sealed class Return : BaseEntity
{
    public const int ReasonMaxLength = 1000;

    private readonly List<ReturnItem> _items = new();

    private Return()
    {
    }

    private Return(string returnNumber, Guid customerId, DateOnly date, string reason)
    {
        Guard.AgainstNullOrWhiteSpace(returnNumber, nameof(returnNumber));
        Guard.AgainstEmptyGuid(customerId, nameof(customerId));
        Guard.AgainstDefaultDate(date, nameof(date));
        Guard.AgainstNullOrWhiteSpace(reason, nameof(reason));
        var rn = returnNumber.Trim();
        var rs = reason.Trim();
        if (rs.Length > ReasonMaxLength) throw new DomainException($"Return reason cannot exceed {ReasonMaxLength} characters.");
        ReturnNumber = rn;
        CustomerId = customerId;
        Date = date;
        Reason = rs;
        Status = ReturnStatuses.Draft;
    }

    public static Return Create(string returnNumber, Guid customerId, DateOnly date, string reason) => new Return(returnNumber, customerId, date, reason);

    public string ReturnNumber { get; private set; } = string.Empty;
    public Guid CustomerId { get; private set; }
    public Customer? Customer { get; internal set; }
    public DateOnly Date { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public string Status { get; private set; } = ReturnStatuses.Draft;
    public IReadOnlyCollection<ReturnItem> Items => _items.AsReadOnly();

    public ReturnItem AddItem(Guid specificationId, decimal qty, string disposition)
    {
        EnsureDraft();

        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstNonPositive(qty, nameof(qty));
        Guard.AgainstNullOrWhiteSpace(disposition, nameof(disposition));

        var item = ReturnItem.Create(Id, specificationId, qty, disposition.Trim());
        _items.Add(item);
        return item;
    }

    public void MarkAsReceived()
    {
        if (Status != ReturnStatuses.Draft) throw new DomainException("Only draft returns can be marked as received.");
        if (!_items.Any()) throw new DomainException("Cannot receive an empty return.");

        Status = ReturnStatuses.Received;
        AddDomainEvent(new ReturnReceived(Id, DateTime.UtcNow));
        // TODO: application layer should credit FinishedGoodsInventory.Receive for appropriate warehouse(s) per returned items.
    }

    public void Cancel()
    {
        if (Status != ReturnStatuses.Draft) throw new DomainException("Only draft returns can be cancelled.");
        Status = ReturnStatuses.Cancelled;
    }

    private void EnsureDraft()
    {
        if (Status != ReturnStatuses.Draft)
        {
            throw new DomainException("Only draft returns can be modified.");
        }
    }
}

public sealed class ReturnItem : BaseEntity
{
    private ReturnItem()
    {
    }

    private ReturnItem(Guid returnId, Guid specificationId, decimal qty, string disposition)
    {
        Guard.AgainstEmptyGuid(returnId, nameof(returnId));
        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstNonPositive(qty, nameof(qty));
        Guard.AgainstNullOrWhiteSpace(disposition, nameof(disposition));

        ReturnId = returnId;
        SpecificationId = specificationId;
        Qty = qty;
        Disposition = disposition;
    }

    public static ReturnItem Create(Guid returnId, Guid specificationId, decimal qty, string disposition)
        => new ReturnItem(returnId, specificationId, qty, disposition);

    public Guid ReturnId { get; private set; }
    public Return? Return { get; internal set; }
    public Guid SpecificationId { get; private set; }
    public Specification? Specification { get; internal set; }
    public decimal Qty { get; private set; }
    public string Disposition { get; private set; } = string.Empty;
}

public static class ReturnStatuses
{
    public const string Draft = "Draft";
    public const string Received = "Received";
    public const string Cancelled = "Cancelled";
}
