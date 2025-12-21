using FluentValidation;
using MyFactory.Application.Features.Payroll.Commands.ClosePayrollPeriod;

namespace MyFactory.Application.OldFeatures.Payroll.Validators;

public sealed class ClosePayrollPeriodCommandValidator : AbstractValidator<ClosePayrollPeriodCommand>
{
    public ClosePayrollPeriodCommandValidator()
    {
        RuleFor(command => command.FromDate)
            .Must(date => date != default)
            .WithMessage("From date is required.");

        RuleFor(command => command.ToDate)
            .Must(date => date != default)
            .WithMessage("To date is required.")
            .GreaterThanOrEqualTo(command => command.FromDate)
            .WithMessage("To date cannot be before from date.");
    }
}
