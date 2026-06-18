using MediatR;

namespace MyFactory.Application.Features.ProductionOrders.CancelProductionOrder;

public sealed record CancelProductionOrderCommand(Guid ProductionOrderId) : IRequest;
