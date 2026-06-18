using MediatR;

namespace MyFactory.Application.Features.ProductionOrders.StartNextProductionStage;

public sealed record StartNextProductionStageCommand(Guid ProductionOrderId) : IRequest;
