using System;

namespace MyFactory.MauiClient.Models.Shipments;

public record ShipmentCardResponse(
    Guid ShipmentId,
    string Customer,
    DateTime Date,
    ShipmentStatus Status,
    decimal TotalAmount,
    ShipmentItemDto[] Items
);
