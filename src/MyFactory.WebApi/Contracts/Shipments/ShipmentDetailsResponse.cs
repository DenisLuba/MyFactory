namespace MyFactory.WebApi.Contracts.Shipments;

public sealed record ShipmentDetailsResponse(
    Guid Id,
    Guid SalesOrderId,
    Guid CustomerId,
    DateTime ShipmentDate,
    ShipmentStatusResponse? Status,
    IReadOnlyCollection<ShipmentDetailsItemResponse> Items
);
