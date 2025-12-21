using System;
using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Production;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.OldFeatures.Production.Queries.GetProductionOrders;

public sealed record GetProductionOrdersQuery(
    string? Status,
    Guid? SpecificationId) : IRequest<IReadOnlyCollection<ProductionOrderDto>>;
