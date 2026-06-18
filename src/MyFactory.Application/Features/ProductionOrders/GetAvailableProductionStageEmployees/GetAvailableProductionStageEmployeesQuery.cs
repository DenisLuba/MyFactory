using MediatR;
using MyFactory.Application.DTOs.Employees;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.GetAvailableProductionStageEmployees;

public sealed record GetAvailableProductionStageEmployeesQuery(
    Guid ProductionOrderId,
    ProductionStage Stage
) : IRequest<IReadOnlyList<EmployeeListItemDto>>;
