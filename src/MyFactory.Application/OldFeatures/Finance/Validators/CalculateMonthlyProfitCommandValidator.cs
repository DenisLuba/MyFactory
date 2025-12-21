using FluentValidation;
using MyFactory.Application.Features.Finance.Commands.MonthlyProfit;

namespace MyFactory.Application.OldFeatures.Finance.Validators;

public sealed class CalculateMonthlyProfitCommandValidator : AbstractValidator<CalculateMonthlyProfitCommand>
{
    public CalculateMonthlyProfitCommandValidator()
    {
        RuleFor(command => command.PeriodMonth).InclusiveBetween(1, 12);
        RuleFor(command => command.PeriodYear).GreaterThanOrEqualTo(2000);
    }
}
