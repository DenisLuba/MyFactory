using System;
using MediatR;
using MyFactory.Application.DTOs.FinishedGoods;

namespace MyFactory.Application.Features.FinishedGoods.Queries.GetShipmentById;

public sealed record GetShipmentByIdQuery(Guid ShipmentId) : IRequest<ShipmentDto>;
