using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Exceptions;

namespace MyFactory.Domain.Entities.Orders;

public class SalesOrderEntity : AuditableEntity
{
    public string OrderNumber { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public DateTime? RequiredByDate { get; private set; }
    public SalesOrderStatus Status { get; private set; }
    public Guid CreatedBy { get; private set; }

    public IReadOnlyCollection<SalesOrderItemEntity> SalesOrderItems { get; private set; } = new List<SalesOrderItemEntity>();
    public IReadOnlyCollection<ShipmentEntity> Shipments { get; private set; } = new List<ShipmentEntity>();
    public IReadOnlyCollection<ShipmentReturnEntity> ShipmentReturns { get; private set; } = new List<ShipmentReturnEntity>();

    public SalesOrderEntity(string orderNumber, Guid customerId, DateTime orderDate, Guid createdBy)
    {
        Guard.AgainstNullOrWhiteSpace(orderNumber, nameof(orderNumber));
        Guard.AgainstEmptyGuid(customerId, nameof(customerId));
        Guard.AgainstDefaultDate(orderDate, nameof(orderDate));
        Guard.AgainstEmptyGuid(createdBy, nameof(createdBy));

        OrderNumber = orderNumber;
        CustomerId = customerId;
        OrderDate = orderDate;
        CreatedBy = createdBy;
        Status = SalesOrderStatus.New;
    }

    public static SalesOrderEntity Create(string orderNumber, Guid customerId, DateTime orderDate, Guid createdBy)
    {
        return new SalesOrderEntity(orderNumber, customerId, orderDate, createdBy);
    }

    public void Confirm()
    {
        if (Status != SalesOrderStatus.New)
            throw new DomainException("Only new orders can be confirmed.");
        Status = SalesOrderStatus.Confirmed;
        Touch();
    }

    public void Cancel()
    {
        if (Status == SalesOrderStatus.Fulfilled || Status == SalesOrderStatus.Cancelled)
            throw new DomainException("Cannot cancel a fulfilled or already cancelled order.");
        Status = SalesOrderStatus.Cancelled;
        Touch();
    }

    public void Fulfill()
    {
        if (Status != SalesOrderStatus.Confirmed && Status != SalesOrderStatus.PartiallyFulfilled)
            throw new DomainException("Order must be confirmed or partially fulfilled to be fulfilled.");
        Status = SalesOrderStatus.Fulfilled;
        Touch();
    }

    public void SetRequiredByDate(DateTime? requiredByDate)
    {
        if (requiredByDate.HasValue && requiredByDate.Value < OrderDate)
            throw new DomainException("RequiredByDate cannot be earlier than OrderDate.");

        RequiredByDate = requiredByDate;
        Touch();
    }

    public void Update(Guid customerId, DateTime orderDate)
    {
        Guard.AgainstEmptyGuid(customerId, nameof(customerId));
        Guard.AgainstDefaultDate(orderDate, nameof(orderDate));
        CustomerId = customerId;
        OrderDate = orderDate;
        Touch();
    }
}

public enum SalesOrderStatus
{
    New,
    Confirmed,
    PartiallyFulfilled,
    Fulfilled,
    Cancelled
}

public class SalesOrderItemEntity : AuditableEntity
{
    public Guid SalesOrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public decimal QtyOrdered { get; private set; }
    public decimal QtyAllocated { get; private set; }
    public decimal QtyShipped { get; private set; }
    public decimal? UnitPrice { get; private set; }
    public SalesOrderItemStatus Status { get; private set; }

    public IReadOnlyCollection<ProductionOrderEntity> ProductionOrders { get; private set; } = new List<ProductionOrderEntity>();
    public IReadOnlyCollection<ShipmentItemEntity> ShipmentItems { get; private set; } = new List<ShipmentItemEntity>();
    public IReadOnlyCollection<ShipmentReturnItemEntity> ShipmentReturnItems { get; private set; } = new List<ShipmentReturnItemEntity>();

    public SalesOrderItemEntity(Guid salesOrderId, Guid productId, decimal qtyOrdered)
    {
        Guard.AgainstEmptyGuid(salesOrderId, nameof(salesOrderId));
        Guard.AgainstEmptyGuid(productId, nameof(productId));
        Guard.AgainstNonPositive(qtyOrdered, nameof(qtyOrdered));

        SalesOrderId = salesOrderId;
        ProductId = productId;
        QtyOrdered = qtyOrdered;
        Status = SalesOrderItemStatus.New;
    }

    public void SetUnitPrice(decimal? unitPrice)
    {
        if (unitPrice.HasValue)
            Guard.AgainstNegative(unitPrice.Value, nameof(unitPrice));
        UnitPrice = unitPrice;
        Touch();
    }

    public void SetStatus(SalesOrderItemStatus status)
    {
        Status = status;
        Touch();
    }

    public void UpdateQty(decimal qtyOrdered)
    {
        Guard.AgainstNonPositive(qtyOrdered, nameof(qtyOrdered));
        QtyOrdered = qtyOrdered;
        Touch();
    }
}

public enum SalesOrderItemStatus
{
    New,
    Cut,
    Sewing,
    Package,
    Finished,
    Shipped,
    Cancelled
}

public class ShipmentEntity : AuditableEntity
{
    public Guid SalesOrderId { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateTime ShipmentDate { get; private set; }
    public ShipmentStatus Status { get; private set; }
    public Guid CreatedBy { get; private set; }

    public IReadOnlyCollection<ShipmentItemEntity> ShipmentItems { get; private set; } = new List<ShipmentItemEntity>();
    public IReadOnlyCollection<ShipmentReturnEntity> ShipmentReturns { get; private set; } = new List<ShipmentReturnEntity>();

    public ShipmentEntity(Guid salesOrderId, Guid customerId, DateTime shipmentDate, Guid createdBy)
    {
        Guard.AgainstEmptyGuid(salesOrderId, nameof(salesOrderId));
        Guard.AgainstEmptyGuid(customerId, nameof(customerId));
        Guard.AgainstDefaultDate(shipmentDate, nameof(shipmentDate));
        Guard.AgainstEmptyGuid(createdBy, nameof(createdBy));

        SalesOrderId = salesOrderId;
        CustomerId = customerId;
        ShipmentDate = shipmentDate;
        CreatedBy = createdBy;
        Status = ShipmentStatus.Draft;
    }

    public void Confirm()
    {
        if (Status != ShipmentStatus.Draft)
            throw new DomainException("Only draft shipments can be confirmed.");
        Status = ShipmentStatus.Confirmed;
        Touch();
    }

    public void Ship()
    {
        if (Status != ShipmentStatus.Confirmed)
            throw new DomainException("Only confirmed shipments can be shipped.");
        Status = ShipmentStatus.Shipped;
        Touch();
    }

    public void Cancel()
    {
        if (Status == ShipmentStatus.Shipped || Status == ShipmentStatus.Cancelled)
            throw new DomainException("Cannot cancel a shipped or already cancelled shipment.");
        Status = ShipmentStatus.Cancelled;
        Touch();
    }
}

public enum ShipmentStatus
{
    Draft,
    Confirmed,
    Shipped,
    Cancelled
}

public class ShipmentItemEntity : AuditableEntity
{
    public Guid ShipmentId { get; private set; }
    public Guid SalesOrderItemId { get; private set; }
    public Guid WarehouseId { get; private set; }
    public Guid ProductId { get; private set; }
    public decimal Qty { get; private set; }
    public decimal UnitPrice { get; private set; }

    public IReadOnlyCollection<ShipmentReturnItemEntity> ShipmentReturnItems { get; private set; } = new List<ShipmentReturnItemEntity>();

    public ShipmentItemEntity(Guid shipmentId, Guid salesOrderItemId, Guid warehouseId, Guid productId, int qty, decimal unitPrice)
    {
        Guard.AgainstEmptyGuid(shipmentId, nameof(shipmentId));
        Guard.AgainstEmptyGuid(salesOrderItemId, nameof(salesOrderItemId));
        Guard.AgainstEmptyGuid(warehouseId, nameof(warehouseId));
        Guard.AgainstEmptyGuid(productId, nameof(productId));
        Guard.AgainstNonPositive(qty, nameof(qty));
        Guard.AgainstNegative(unitPrice, nameof(unitPrice));

        ShipmentId = shipmentId;
        SalesOrderItemId = salesOrderItemId;
        WarehouseId = warehouseId;
        ProductId = productId;
        Qty = qty;
        UnitPrice = unitPrice;
    }
}

public class ShipmentReturnEntity : AuditableEntity
{
    public Guid? ShipmentId { get; private set; }
    public Guid SalesOrderId { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateTime ReturnDate { get; private set; }
    public string? Reason { get; private set; }
    public ReturnStatus Status { get; private set; }
    public Guid CreatedBy { get; private set; }

    public IReadOnlyCollection<ShipmentReturnItemEntity> ShipmentReturnItems { get; private set; } = new List<ShipmentReturnItemEntity>();

    public ShipmentReturnEntity(Guid salesOrderId, Guid customerId, DateTime returnDate, Guid createdBy)
    {
        Guard.AgainstEmptyGuid(salesOrderId, nameof(salesOrderId));
        Guard.AgainstEmptyGuid(customerId, nameof(customerId));
        Guard.AgainstDefaultDate(returnDate, nameof(returnDate));
        Guard.AgainstEmptyGuid(createdBy, nameof(createdBy));

        SalesOrderId = salesOrderId;
        CustomerId = customerId;
        ReturnDate = returnDate;
        CreatedBy = createdBy;
        Status = ReturnStatus.New;
    }

    public void SetReason(string? reason)
    {
        Reason = reason;
        Touch();
    }

    public void SetStatus(ReturnStatus status)
    {
        Status = status;
        Touch();
    }
}

public enum ReturnStatus
{
    New,
    Processing,
    Accepted,
    Rejected,
    Cancelled
}

public class ShipmentReturnItemEntity : AuditableEntity
{
    public Guid ShipmentReturnId { get; private set; }
    public Guid? ShipmentItemId { get; private set; }
    public Guid SalesOrderItemId { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid WarehouseId { get; private set; }
    public int Qty { get; private set; }
    public decimal UnitPrice { get; private set; }
    public ReturnCondition Condition { get; private set; }

    public ShipmentReturnItemEntity(
        Guid shipmentReturnId,
        Guid salesOrderItemId,
        Guid productId,
        Guid warehouseId,
        int qty,
        decimal unitPrice,
        ReturnCondition condition,
        Guid? shipmentItemId = null)
    {
        Guard.AgainstEmptyGuid(shipmentReturnId, nameof(shipmentReturnId));
        Guard.AgainstEmptyGuid(salesOrderItemId, nameof(salesOrderItemId));
        Guard.AgainstEmptyGuid(productId, nameof(productId));
        Guard.AgainstEmptyGuid(warehouseId, nameof(warehouseId));
        Guard.AgainstNonPositive(qty, nameof(qty));
        Guard.AgainstNegative(unitPrice, nameof(unitPrice));

        ShipmentReturnId = shipmentReturnId;
        SalesOrderItemId = salesOrderItemId;
        ProductId = productId;
        WarehouseId = warehouseId;
        Qty = qty;
        UnitPrice = unitPrice;
        Condition = condition;
        ShipmentItemId = shipmentItemId;
    }
}

public enum ReturnCondition
{
    New,
    Opened,
    Used,
    Damaged
}