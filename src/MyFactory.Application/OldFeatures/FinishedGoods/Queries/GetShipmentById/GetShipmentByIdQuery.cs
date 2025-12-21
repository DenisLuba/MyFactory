using System;
using MediatR;
using MyFactory.Application.DTOs.FinishedGoods;

namespace MyFactory.Application.OldFeatures.FinishedGoods.Queries.GetShipmentById;

public sealed record GetShipmentByIdQuery(Guid ShipmentId) : IRequest<ShipmentDto>;
