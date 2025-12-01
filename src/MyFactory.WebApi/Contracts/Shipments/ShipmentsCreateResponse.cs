using System;

namespace MyFactory.WebApi.Contracts.Shipments;

public record ShipmentsCreateResponse(
    Guid ShipmentId,
    ShipmentStatus Status
);

