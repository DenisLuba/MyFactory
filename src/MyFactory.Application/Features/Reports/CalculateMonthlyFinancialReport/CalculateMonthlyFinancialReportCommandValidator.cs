using FluentValidation;

namespace MyFactory.Application.Features.Reports.CalculateMonthlyFinancialReport;

public sealed class CalculateMonthlyFinancialReportCommandValidator
    : AbstractValidator<CalculateMonthlyFinancialReportCommand>
{
    public CalculateMonthlyFinancialReportCommandValidator()
    {
        RuleFor(x => x.Year)
            .InclusiveBetween(2000, 2100);

        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12);
    }
}
