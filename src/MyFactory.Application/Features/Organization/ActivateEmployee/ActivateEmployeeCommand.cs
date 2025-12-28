using MediatR;

namespace MyFactory.Application.Features.ActivateEmployee;

public sealed record ActivateEmployeeCommand(
    Guid EmployeeId,
    DateTime HiredAt
) : IRequest;