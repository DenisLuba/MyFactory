using System;
using MediatR;
using MyFactory.Application.DTOs.Shipments;

namespace MyFactory.Application.Features.Shipments.CreateShipment;

public sealed record CreateShipmentCommand(
    Guid SalesOrderId,
    Guid CustomerId,
    DateTime ShipmentDate,
    Guid CreatedBy,
    ShipmentStatus? Status,
    IReadOnlyCollection<CreateShipmentItemDto> Items
) : IRequest<Guid>;


