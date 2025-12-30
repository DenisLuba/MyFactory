using MediatR;

namespace MyFactory.Application.Features.Employees.UpdateEmployee;

public sealed record UpdateEmployeeCommand(
    Guid EmployeeId,
    string FullName,
    Guid PositionId,
    int Grade,
    decimal RatePerNormHour,
    decimal PremiumPercent,
    DateTime HiredAt,
    bool IsActive
) : IRequest;