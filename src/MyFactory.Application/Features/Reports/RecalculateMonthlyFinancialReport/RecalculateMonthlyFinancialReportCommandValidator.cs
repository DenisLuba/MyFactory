using FluentValidation;

namespace MyFactory.Application.Features.Reports.RecalculateMonthlyFinancialReport;

public sealed class RecalculateMonthlyFinancialReportCommandValidator
    : AbstractValidator<RecalculateMonthlyFinancialReportCommand>
{
    public RecalculateMonthlyFinancialReportCommandValidator()
    {
        RuleFor(x => x.Year)
            .InclusiveBetween(2000, 2100);

        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12);
    }
}
