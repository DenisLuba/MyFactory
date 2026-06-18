using MediatR;
using MyFactory.Application.DTOs.ProductionOrders;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionStageEmployees;

public sealed record GetProductionStageEmployeesQuery(
    Guid ProductionOrderId,
    ProductionStage Stage
) : IRequest<IReadOnlyList<ProductionStageAssignmentDto>>;
