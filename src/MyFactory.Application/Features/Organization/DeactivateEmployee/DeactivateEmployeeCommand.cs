using MediatR;

namespace MyFactory.Application.Features.DeactivateEmployee;

public sealed record DeactivateEmployeeCommand(
    Guid EmployeeId,
    DateTime FiredAt
) : IRequest;