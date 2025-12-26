using MediatR;
using MyFactory.Application.DTOs.ProductionOrders;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrderDetails;

public sealed record GetProductionOrderDetailsQuery(Guid ProductionOrderId)
    : IRequest<ProductionOrderDetailsDto>;
