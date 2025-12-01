using System;

namespace MyFactory.MauiClient.Models.Shipments;

public record ShipmentsCreateResponse(
    Guid ShipmentId,
    ShipmentStatus Status
);

