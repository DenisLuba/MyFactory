using FluentValidation;
using MyFactory.Application.Features.Finance.Commands.OverheadMonthly;

namespace MyFactory.Application.OldFeatures.Finance.Validators;

public sealed class RecordOverheadCommandValidator : AbstractValidator<RecordOverheadCommand>
{
    public RecordOverheadCommandValidator()
    {
        RuleFor(command => command.PeriodMonth).InclusiveBetween(1, 12);
        RuleFor(command => command.PeriodYear).GreaterThanOrEqualTo(2000);
        RuleFor(command => command.ExpenseTypeId).NotEmpty();
        RuleFor(command => command.Amount).GreaterThanOrEqualTo(0m);
    }
}
