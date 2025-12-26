using MediatR;

namespace MyFactory.Application.Features.ProductionOrders.UpdateProductionOrder;

public sealed record UpdateProductionOrderCommand : IRequest
{
    public Guid ProductionOrderId { get; init; }
    public Guid DepartmentId { get; init; }
    public int QtyPlanned { get; init; }
}
