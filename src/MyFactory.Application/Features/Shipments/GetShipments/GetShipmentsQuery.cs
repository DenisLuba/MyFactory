using System;
using MediatR;
using MyFactory.Application.DTOs.Shipments;

namespace MyFactory.Application.Features.Shipments.GetShipments;

public sealed record GetShipmentsQuery(
    Guid? SalesOrderId = null,
    Guid? CustomerId = null,
    ShipmentStatus? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null
) : IRequest<IReadOnlyList<ShipmentListItemDto>>;


