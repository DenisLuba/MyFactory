using MediatR;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.RemoveProductionStageEmployee;

public sealed record RemoveProductionStageEmployeeCommand(
    Guid ProductionOrderId,
    Guid AssignmentId,
    ProductionStage Stage
) : IRequest;
