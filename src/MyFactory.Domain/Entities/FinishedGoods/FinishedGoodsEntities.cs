using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.Entities.Warehousing;
using QuantityValue = MyFactory.Domain.ValueObjects.Quantity;

namespace MyFactory.Domain.Entities.FinishedGoods;

/// <summary>
/// Aggregate root representing finished goods stock for a specification in a warehouse.
/// </summary>
public sealed class FinishedGoodsInventory : BaseEntity
{
    private FinishedGoodsInventory()
    {
    }

    public FinishedGoodsInventory(Guid specificationId, Guid warehouseId)
    {
        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstEmptyGuid(warehouseId, nameof(warehouseId));

        SpecificationId = specificationId;
        WarehouseId = warehouseId;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid SpecificationId { get; private set; }
    public Specification? Specification { get; private set; }
    public Guid WarehouseId { get; private set; }
    public Warehouse? Warehouse { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal UnitCost { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void Receive(decimal quantity, decimal unitCost, DateTime receivedAt)
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

    public void Issue(decimal quantity, DateTime issuedAt)
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
        DateTime movedAt,
        Guid? finishedGoodsInventoryId,
        decimal? sourceAvailableQuantity)
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
    public DateTime MovedAt { get; private set; }
    public Guid? FinishedGoodsInventoryId { get; private set; }
    public FinishedGoodsInventory? FinishedGoodsInventory { get; private set; }

    public static FinishedGoodsMovement CreateTransfer(
        Guid specificationId,
        Guid fromWarehouseId,
        Guid toWarehouseId,
        decimal quantity,
        DateTime movedAt,
        Guid? finishedGoodsInventoryId = null,
        decimal? sourceAvailableQuantity = null)
        => new(specificationId, fromWarehouseId, toWarehouseId, quantity, movedAt, finishedGoodsInventoryId, sourceAvailableQuantity);

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
}
