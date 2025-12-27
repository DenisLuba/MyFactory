using MediatR;
using MyFactory.Domain.Entities.Production;
using MyFactory.Application.DTOs.ProductionOrders;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionStageEmployees;

public sealed record GetProductionStageEmployeesQuery(
    Guid ProductionOrderId,
    ProductionOrderStatus Stage
) : IRequest<IReadOnlyList<ProductionStageEmployeeDto>>;
