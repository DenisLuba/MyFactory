using MediatR;

namespace MyFactory.Application.Features.ProductionOrders.RegisterSewingOperation;

public sealed record RegisterSewingOperationCommand(
    Guid ProductionOrderId,
    Guid AssignmentId,
    Guid EmployeeId,
    decimal QtySewn,
    decimal HoursWorked,
    DateOnly OperationDate
) : IRequest<Guid>;
