using System;

namespace MyFactory.MauiClient.Models.Shipments;

public record ShipmentsListResponse(
    Guid ShipmentId,
    string Customer,
    string ProductName,
    int Quantity,
    DateTime Date,
    decimal TotalAmount,
    ShipmentStatus Status
);
