using System;
using MediatR;
using MyFactory.Application.DTOs.Shipments;

namespace MyFactory.Application.Features.Shipments.UpdateShipment;

public sealed record UpdateShipmentCommand(
    Guid ShipmentId,
    DateTime? ShipmentDate,
    ShipmentStatus? Status,
    IReadOnlyCollection<CreateShipmentItemDto>? Items
) : IRequest<Guid>;
