using FluentValidation;
using MyFactory.Application.Features.Finance.Commands.RevenueReports;

namespace MyFactory.Application.Features.Finance.Validators;

public sealed class MarkRevenuePaidCommandValidator : AbstractValidator<MarkRevenuePaidCommand>
{
    public MarkRevenuePaidCommandValidator()
    {
        RuleFor(command => command.RevenueReportId).NotEmpty();
        RuleFor(command => command.PaymentDate)
            .Must(date => date != default)
            .WithMessage("Payment date is required.");
    }
}
