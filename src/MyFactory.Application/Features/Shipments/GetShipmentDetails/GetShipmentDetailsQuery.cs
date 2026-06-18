using MediatR;
using MyFactory.Application.DTOs.Shipments;

namespace MyFactory.Application.Features.Shipments.GetShipmentDetails;

public sealed record GetShipmentDetailsQuery(Guid ShipmentId) : IRequest<ShipmentDetailsDto?>;

public sealed record ShipmentDetailsDto(
    Guid Id,
    Guid SalesOrderId,
    Guid CustomerId,
    DateTime ShipmentDate,
    ShipmentStatus? Status,
    IReadOnlyCollection<ShipmentDetailsItemDto> Items
);


