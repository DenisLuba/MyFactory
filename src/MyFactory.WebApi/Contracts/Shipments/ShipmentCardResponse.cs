using System;

namespace MyFactory.WebApi.Contracts.Shipments;

public record ShipmentCardResponse(
    Guid ShipmentId,
    string Customer,
    DateTime Date,
    ShipmentStatus Status,
    decimal TotalAmount,
    ShipmentItemDto[] Items
);
