using MediatR;

namespace MyFactory.Application.Features.Employees.ActivateEmployee;

public sealed record ActivateEmployeeCommand(
    Guid EmployeeId,
    DateTime HiredAt
) : IRequest;