using MediatR;
using MyFactory.Application.DTOs.ProductionOrders;

namespace MyFactory.Application.Features.ProductionOrders.IssueMaterialsToProduction;

public sealed class IssueMaterialsToProductionCommand : IRequest
{
    public Guid ProductionOrderId { get; init; }

    public IReadOnlyList<IssueMaterialLineDto> Materials { get; init; } = [];
}
