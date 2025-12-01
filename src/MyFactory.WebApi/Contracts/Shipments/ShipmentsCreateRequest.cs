using System;

namespace MyFactory.WebApi.Contracts.Shipments;

public record ShipmentsCreateRequest(
    Guid CustomerId,
    ShipmentItemDto[] Items
);

public record ShipmentItemDto(
    Guid SpecificationId,
    string ProductName,
    int Qty,
    decimal UnitPrice,
    decimal LineTotal
);

