using MediatR;

namespace MyFactory.Application.Features.ProductionOrders.DeleteProductionOrder;

public sealed record DeleteProductionOrderCommand(Guid ProductionOrderId) : IRequest;
