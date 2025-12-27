using MediatR;

namespace MyFactory.Application.Features.ProductionOrders.CompleteProductionStage;

public sealed record CompleteProductionStageCommand(Guid ProductionOrderId, int Qty) : IRequest;
