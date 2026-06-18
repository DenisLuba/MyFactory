using MediatR;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.AddProductionStageEmployee;

public sealed record AddProductionStageEmployeeCommand(
    Guid ProductionOrderId,
    ProductionStage Stage,
    Guid EmployeeId,
    decimal AssignedQty,
    decimal CompletedQty,
    DateOnly Date
) : IRequest<Guid>;

