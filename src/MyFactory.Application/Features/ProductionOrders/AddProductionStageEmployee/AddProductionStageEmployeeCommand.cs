using MediatR;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.AddProductionStageEmployee;

public sealed record AddProductionStageEmployeeCommand(
    Guid ProductionOrderId,
    ProductionOrderStatus Stage,
    Guid EmployeeId,
    int QtyPlanned, // если уже есть запланированное количество, то будет добавлено к нему
    int QtyCompleted,
    DateOnly Date,
    decimal? HoursWorked
) : IRequest;
