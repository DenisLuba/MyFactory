using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.FinishedGoods;
using MyFactory.Domain.Entities.Sales;

namespace MyFactory.Application.Features.FinishedGoods.Queries.GetShipments;

public sealed record GetShipmentsQuery(ShipmentStatus? Status) : IRequest<IReadOnlyCollection<ShipmentDto>>;
