using MediatR;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrders;

public sealed record GetProductionOrdersQuery : IRequest<IReadOnlyList<ProductionOrderListItemDto>>;
