using MediatR;

namespace MyFactory.Application.Features.CreateEmployee;

public sealed record CreateEmployeeCommand(
    string FullName,
    Guid PositionId,
    int Grade,
    decimal RatePerNormHour,
    decimal PremiumPercent,
    DateTime HiredAt,
    bool IsActive
) : IRequest<Guid>;