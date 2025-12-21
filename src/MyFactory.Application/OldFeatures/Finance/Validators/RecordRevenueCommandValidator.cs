using FluentValidation;
using MyFactory.Application.Features.Finance.Commands.RevenueReports;

namespace MyFactory.Application.OldFeatures.Finance.Validators;

public sealed class RecordRevenueCommandValidator : AbstractValidator<RecordRevenueCommand>
{
    public RecordRevenueCommandValidator()
    {
        RuleFor(command => command.PeriodMonth).InclusiveBetween(1, 12);
        RuleFor(command => command.PeriodYear).GreaterThanOrEqualTo(2000);
        RuleFor(command => command.SpecificationId).NotEmpty();
        RuleFor(command => command.Quantity).GreaterThanOrEqualTo(0m);
        RuleFor(command => command.UnitPrice).GreaterThanOrEqualTo(0m);

        When(command => command.IsPaid, () =>
        {
            RuleFor(command => command.PaymentDate)
                .NotNull()
                .Must(date => date != null && date.Value != default)
                .WithMessage("Payment date is required when revenue is marked as paid.");
        });
    }
}
