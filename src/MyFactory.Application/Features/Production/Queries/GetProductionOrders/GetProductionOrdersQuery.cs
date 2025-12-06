using System;
using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Production;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.Production.Queries.GetProductionOrders;

public sealed record GetProductionOrdersQuery(
    ProductionOrderStatus? Status,
    Guid? SpecificationId) : IRequest<IReadOnlyCollection<ProductionOrderDto>>;
