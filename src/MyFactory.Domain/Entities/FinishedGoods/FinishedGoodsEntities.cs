using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.Entities.Warehousing;

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

    public Guid SpecificationId { get; }
    public Specification? Specification { get; private set; }
    public Guid WarehouseId { get; }
    public Warehouse? Warehouse { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal UnitCost { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void Receive(decimal quantity, decimal unitCost, DateTime receivedAt)
    {
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Guard.AgainstNonPositive(unitCost, nameof(unitCost));
        Guard.AgainstDefaultDate(receivedAt, nameof(receivedAt));

        var totalValue = (Quantity * UnitCost) + (quantity * unitCost);
        var newQuantity = Quantity + quantity;
        UnitCost = totalValue / newQuantity;
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

    private FinishedGoodsMovement(Guid specificationId, Guid fromWarehouseId, Guid toWarehouseId, decimal quantity, DateTime movedAt)
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
    }

    public Guid SpecificationId { get; private set; }
    public Specification? Specification { get; private set; }
    public Guid FromWarehouseId { get; private set; }
    public Warehouse? FromWarehouse { get; private set; }
    public Guid ToWarehouseId { get; private set; }
    public Warehouse? ToWarehouse { get; private set; }
    public decimal Quantity { get; private set; }
    public DateTime MovedAt { get; private set; }

    public static FinishedGoodsMovement CreateTransfer(Guid specificationId, Guid fromWarehouseId, Guid toWarehouseId, decimal quantity, DateTime movedAt)
        => new(specificationId, fromWarehouseId, toWarehouseId, quantity, movedAt);
}
