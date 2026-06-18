using MediatR;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.StartProductionStage;

public sealed record StartProductionStageCommand(Guid ProductionOrderId, ProductionOrderStatus TargetStatus) : IRequest;
