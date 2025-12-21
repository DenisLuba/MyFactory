using System;
using MediatR;
using MyFactory.Application.DTOs.Production;

namespace MyFactory.Application.OldFeatures.Production.Queries.GetProductionOrderById;

public sealed record GetProductionOrderByIdQuery(Guid Id) : IRequest<ProductionOrderDto>;
