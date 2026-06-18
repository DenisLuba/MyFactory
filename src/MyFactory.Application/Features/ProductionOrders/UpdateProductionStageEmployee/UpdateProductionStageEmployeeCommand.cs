using MediatR;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.UpdateProductionStageEmployee;

public sealed record UpdateProductionStageEmployeeCommand(
    Guid AssignmentId,
    ProductionStage Stage,
    Guid ProductionOrderId,
    Guid EmployeeId,
    decimal AssignedQty,
    decimal CompletedQty,
    DateOnly Date
) : IRequest;
