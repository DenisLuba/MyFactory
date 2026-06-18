using MediatR;

namespace MyFactory.Application.Features.Finance.CalculateDailyPayrollAccrual;

public sealed record CalculateDailyPayrollAccrualCommand(
    Guid EmployeeId,
    DateOnly Date
) : IRequest;
