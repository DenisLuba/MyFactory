using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.Entities.Warehousing;
using DescriptionValue = MyFactory.Domain.ValueObjects.Description;
using DocumentNumberValue = MyFactory.Domain.ValueObjects.DocumentNumber;
using MoneyValue = MyFactory.Domain.ValueObjects.Money;
using QuantityValue = MyFactory.Domain.ValueObjects.Quantity;

namespace MyFactory.Domain.Entities.FinishedGoods;

/// <summary>
/// Aggregate root representing finished goods stock for a specification in a warehouse.
/// </summary>
public sealed class FinishedGoodsInventory : BaseEntity
{
    private readonly List<FinishedGoodsMovement> _movements = new();

    private FinishedGoodsInventory()
    {
    }

    public FinishedGoodsInventory(Guid specificationId, Guid warehouseId)
    {
        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstEmptyGuid(warehouseId, nameof(warehouseId));

        SpecificationId = specificationId;
        WarehouseId = warehouseId;
        UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public Guid SpecificationId { get; private set; }
    public Specification? Specification { get; private set; }
    public Guid WarehouseId { get; private set; }
    public Warehouse? Warehouse { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal UnitCost { get; private set; }
    public DateOnly UpdatedAt { get; private set; }
    public IReadOnlyCollection<FinishedGoodsMovement> Movements => _movements.AsReadOnly();

    public void Receive(decimal quantity, decimal unitCost, DateOnly receivedAt)
    {
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Guard.AgainstNegative(unitCost, nameof(unitCost));
        Guard.AgainstDefaultDate(receivedAt, nameof(receivedAt));

        QuantityValue.From(quantity);
        MoneyValue.From(unitCost);

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
        QuantityValue.From(quantity);
        if (quantity > Quantity)
        {
            throw new DomainException("Cannot issue more than on-hand quantity.");
        }

        Quantity -= quantity;
        UpdatedAt = issuedAt;
    }

    public FinishedGoodsMovement MoveToWarehouse(Guid toWarehouseId, decimal quantity, DateOnly movedAt, string? reason = null)
    {
        Guard.AgainstEmptyGuid(toWarehouseId, nameof(toWarehouseId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Guard.AgainstDefaultDate(movedAt, nameof(movedAt));
        QuantityValue.From(quantity);
        if (toWarehouseId == WarehouseId)
        {
            throw new DomainException("Destination warehouse must differ from the source warehouse.");
        }

        var availableBeforeMove = Quantity;
        Issue(quantity, movedAt);

        var movement = FinishedGoodsMovement.CreateTransfer(
            SpecificationId,
            WarehouseId,
            toWarehouseId,
            quantity,
            movedAt,
            Id,
            availableBeforeMove,
            reason);

        movement.AttachSourceInventory(this, availableBeforeMove);
        return movement;
    }

    internal void RegisterMovement(FinishedGoodsMovement movement)
    {
        Guard.AgainstNull(movement, nameof(movement));
        if (movement.FinishedGoodsInventoryId.HasValue && movement.FinishedGoodsInventoryId.Value != Id)
        {
            throw new DomainException("Movement reference mismatch for finished goods inventory.");
        }

        if (_movements.Any(existing => existing.Id == movement.Id))
        {
            return;
        }

        _movements.Add(movement);
    }
}

/// <summary>
/// Domain event record tracking movement of finished goods between warehouses.
/// </summary>
public sealed class FinishedGoodsMovement : BaseEntity
{
    private FinishedGoodsMovement()
    {
    }

    private FinishedGoodsMovement(
        Guid specificationId,
        Guid fromWarehouseId,
        Guid toWarehouseId,
        decimal quantity,
        DateOnly movedAt,
        Guid? finishedGoodsInventoryId,
        decimal? sourceAvailableQuantity,
        string? reason)
    {
        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstEmptyGuid(fromWarehouseId, nameof(fromWarehouseId));
        Guard.AgainstEmptyGuid(toWarehouseId, nameof(toWarehouseId));
        QuantityValue.From(quantity);
        Guard.AgainstDefaultDate(movedAt, nameof(movedAt));

        if (fromWarehouseId == toWarehouseId)
        {
            throw new DomainException("Source and destination warehouses must differ.");
        }

        if (sourceAvailableQuantity.HasValue && quantity > sourceAvailableQuantity.Value)
        {
            throw new DomainException("Movement quantity cannot exceed available source quantity.");
        }

        SpecificationId = specificationId;
        FromWarehouseId = fromWarehouseId;
        ToWarehouseId = toWarehouseId;
        Quantity = quantity;
        MovedAt = movedAt;
        Reason = DescriptionValue.From(reason).Value;

        if (finishedGoodsInventoryId.HasValue)
        {
            AttachSourceInventory(finishedGoodsInventoryId.Value, sourceAvailableQuantity ?? quantity);
        }
    }

    public Guid SpecificationId { get; private set; }
    public Specification? Specification { get; private set; }
    public Guid FromWarehouseId { get; private set; }
    public Warehouse? FromWarehouse { get; private set; }
    public Guid ToWarehouseId { get; private set; }
    public Warehouse? ToWarehouse { get; private set; }
    public decimal Quantity { get; private set; }
    public DateOnly MovedAt { get; private set; }
    public Guid? FinishedGoodsInventoryId { get; private set; }
    public FinishedGoodsInventory? FinishedGoodsInventory { get; private set; }
    public string? Reason { get; private set; }

    public static FinishedGoodsMovement CreateTransfer(
        Guid specificationId,
        Guid fromWarehouseId,
        Guid toWarehouseId,
        decimal quantity,
        DateOnly movedAt,
        Guid? finishedGoodsInventoryId = null,
        decimal? sourceAvailableQuantity = null,
        string? reason = null)
        => new(specificationId, fromWarehouseId, toWarehouseId, quantity, movedAt, finishedGoodsInventoryId, sourceAvailableQuantity, reason);


public enum FinishedGoodsReceiptStatus
{
    Draft = 0,
    Accepted = 1,
    Moved = 2
}

/// <summary>
/// Aggregate root that records finished goods receipt from production into a warehouse.
/// </summary>
public sealed class FinishedGoodsReceipt : BaseEntity
{
    private FinishedGoodsReceipt()
    {
    }

    public FinishedGoodsReceipt(
        string documentNumber,
        Guid specificationId,
        Guid warehouseId,
        decimal quantity,
        decimal unitCost,
        DateOnly receiptDate,
        DateOnly? productionDate = null)
    {
        UpdateDocumentNumber(documentNumber);
        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstEmptyGuid(warehouseId, nameof(warehouseId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Guard.AgainstNegative(unitCost, nameof(unitCost));
        Guard.AgainstDefaultDate(receiptDate, nameof(receiptDate));

        SpecificationId = specificationId;
        WarehouseId = warehouseId;
        QuantityValue.From(quantity);
        MoneyValue.From(unitCost);
        Quantity = quantity;
        UnitCost = unitCost;
        ReceiptDate = receiptDate;
        ProductionDate = productionDate;
        Status = FinishedGoodsReceiptStatus.Draft;
    }

    public string DocumentNumber { get; private set; } = string.Empty;
    public Guid SpecificationId { get; private set; }
    public Specification? Specification { get; private set; }
    public Guid WarehouseId { get; private set; }
    public Warehouse? Warehouse { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal UnitCost { get; private set; }
    public decimal TotalAmount => Quantity * UnitCost;
    public DateOnly ReceiptDate { get; private set; }
    public DateOnly? ProductionDate { get; private set; }
    public FinishedGoodsReceiptStatus Status { get; private set; } = FinishedGoodsReceiptStatus.Draft;
    public Guid? FinishedGoodsInventoryId { get; private set; }
    public FinishedGoodsInventory? FinishedGoodsInventory { get; private set; }
    public Guid? MovementId { get; private set; }
    public FinishedGoodsMovement? Movement { get; private set; }
    public string? MoveReason { get; private set; }
    public DateOnly? MovedAt { get; private set; }

    public void Accept(FinishedGoodsInventory inventory, DateOnly acceptedAt)
    {
        Guard.AgainstNull(inventory, nameof(inventory));
        Guard.AgainstDefaultDate(acceptedAt, nameof(acceptedAt));
        EnsureDraft();

        if (inventory.SpecificationId != SpecificationId)
        {
            throw new DomainException("Receipt specification mismatch with inventory.");
        }

        if (inventory.WarehouseId != WarehouseId)
        {
            throw new DomainException("Receipt warehouse mismatch with inventory.");
        }

        inventory.Receive(Quantity, UnitCost, acceptedAt);

        Status = FinishedGoodsReceiptStatus.Accepted;
        ReceiptDate = acceptedAt;
        FinishedGoodsInventoryId = inventory.Id;
        FinishedGoodsInventory = inventory;
    }

    public void MarkAsMoved(FinishedGoodsMovement movement, DateOnly movedAt, string? moveReason = null)
    {
        Guard.AgainstNull(movement, nameof(movement));
        Guard.AgainstDefaultDate(movedAt, nameof(movedAt));
        EnsureAccepted();

        if (movement.SpecificationId != SpecificationId)
        {
            throw new DomainException("Movement specification mismatch with receipt.");
        }

        if (movement.FromWarehouseId != WarehouseId)
        {
            throw new DomainException("Movement must originate from the receipt warehouse.");
        }

        if (movement.Quantity > Quantity)
        {
            throw new DomainException("Movement quantity cannot exceed the received quantity.");
        }

        if (MovementId.HasValue)
        {
            throw new DomainException("Receipt already linked to another movement.");
        }

        MovementId = movement.Id;
        Movement = movement;
        MoveReason = DescriptionValue.From(moveReason).Value;
        MovedAt = movedAt;
        Status = FinishedGoodsReceiptStatus.Moved;
    }

    public void UpdateQuantity(decimal quantity)
    {
        EnsureDraft();
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        QuantityValue.From(quantity);
        Quantity = quantity;
    }

    public void UpdateUnitCost(decimal unitCost)
    {
        EnsureDraft();
        Guard.AgainstNegative(unitCost, nameof(unitCost));
        MoneyValue.From(unitCost);
        UnitCost = unitCost;
    }

    public void UpdateProductionDate(DateOnly? productionDate)
    {
        EnsureDraft();
        ProductionDate = productionDate;
    }

    public void UpdateDocumentNumber(string documentNumber)
    {
        EnsureDraft();
        SetDocumentNumber(documentNumber);
    }

    private void EnsureDraft()
    {
        if (Status != FinishedGoodsReceiptStatus.Draft)
        {
            throw new DomainException("Only draft receipts can be modified.");
        }
    }

    private void EnsureAccepted()
    {
        if (Status != FinishedGoodsReceiptStatus.Accepted)
        {
            throw new DomainException("Receipt must be accepted before it can be moved.");
        }
    }

    private void SetDocumentNumber(string documentNumber)
    {
        DocumentNumber = DocumentNumberValue.From(documentNumber).Value;
    }
}
    public void AttachSourceInventory(Guid finishedGoodsInventoryId, decimal sourceAvailableQuantity)
    {
        Guard.AgainstEmptyGuid(finishedGoodsInventoryId, nameof(finishedGoodsInventoryId));
        AttachSourceInventoryInternal(finishedGoodsInventoryId, null, sourceAvailableQuantity);
    }

    public void AttachSourceInventory(FinishedGoodsInventory inventory, decimal sourceAvailableQuantity)
    {
        Guard.AgainstNull(inventory, nameof(inventory));
        AttachSourceInventoryInternal(inventory.Id, inventory, sourceAvailableQuantity);
    }

    private void AttachSourceInventoryInternal(Guid finishedGoodsInventoryId, FinishedGoodsInventory? inventory, decimal sourceAvailableQuantity)
    {
        if (FinishedGoodsInventoryId.HasValue && FinishedGoodsInventoryId.Value != finishedGoodsInventoryId)
        {
            throw new DomainException("Movement already linked to a different source inventory.");
        }

        if (Quantity > sourceAvailableQuantity)
        {
            throw new DomainException("Movement quantity cannot exceed available source quantity.");
        }

        FinishedGoodsInventoryId = finishedGoodsInventoryId;
        FinishedGoodsInventory = inventory ?? FinishedGoodsInventory;
        inventory?.RegisterMovement(this);
    }
}
