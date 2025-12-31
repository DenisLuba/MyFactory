using MediatR;
using MyFactory.Application.Common.ValueObjects;

namespace MyFactory.Application.Features.Finance.CalculateDailyPayrollAccrualsForPeriod;

public sealed record CalculatePayrollAccrualsForPeriodCommand(
    YearMonth Period
) : IRequest;

