using MediatR;
using MyFactory.Application.DTOs.ProductionOrders;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrderMaterials;

public sealed record GetProductionOrderMaterialsQuery(Guid ProductionOrderId)
    : IRequest<IReadOnlyList<ProductionOrderMaterialDto>>;
