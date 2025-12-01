using System;

namespace MyFactory.WebApi.Contracts.Shipments;

public record ShipmentsListResponse(
    Guid ShipmentId,
    string Customer,
    string ProductName,
    int Quantity,
    DateTime Date,
    decimal TotalAmount,
    ShipmentStatus Status
);
