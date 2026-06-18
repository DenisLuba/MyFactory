using MediatR;

namespace MyFactory.Application.Features.ProductionOrders.CreateProductionOrder;

public sealed record CreateProductionOrderCommand : IRequest<Guid>
{
    public Guid SalesOrderItemId { get; init; }
    public Guid DepartmentId { get; init; }
    public decimal QtyPlanned { get; init; }
}
