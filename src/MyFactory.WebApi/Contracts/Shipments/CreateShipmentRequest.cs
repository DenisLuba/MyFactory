namespace MyFactory.WebApi.Contracts.Shipments;

public sealed record CreateShipmentRequest(
    Guid SalesOrderId,
    Guid CustomerId,
    DateTime ShipmentDate,
    Guid CreatedBy,
    ShipmentStatusResponse? Status,
    IReadOnlyCollection<CreateShipmentItemRequest> Items
);
