using FluentValidation;
using MyFactory.Application.Features.Payroll.Queries.GetPayrollForPeriod;

namespace MyFactory.Application.OldFeatures.Payroll.Validators;

public sealed class GetPayrollForPeriodQueryValidator : AbstractValidator<GetPayrollForPeriodQuery>
{
    public GetPayrollForPeriodQueryValidator()
    {
        RuleFor(query => query.FromDate)
            .Must(date => date != default)
            .WithMessage("From date is required.");

        RuleFor(query => query.ToDate)
            .Must(date => date != default)
            .WithMessage("To date is required.")
            .GreaterThanOrEqualTo(query => query.FromDate)
            .WithMessage("To date cannot be before from date.");
    }
}
