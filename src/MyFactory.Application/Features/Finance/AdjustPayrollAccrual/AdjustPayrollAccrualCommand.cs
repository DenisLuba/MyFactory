using MediatR;

namespace MyFactory.Application.Features.Finance.AdjustPayrollAccrual;

public sealed record AdjustPayrollAccrualCommand(
    Guid AccrualId,
    decimal BaseAmount,
    decimal PremiumAmount,
    string Reason
) : IRequest;
