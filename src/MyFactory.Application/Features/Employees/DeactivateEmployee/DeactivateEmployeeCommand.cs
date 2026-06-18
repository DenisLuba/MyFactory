using MediatR;

namespace MyFactory.Application.Features.Employees.DeactivateEmployee;

public sealed record DeactivateEmployeeCommand(
    Guid EmployeeId,
    DateTime FiredAt
) : IRequest;