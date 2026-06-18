using FluentValidation;

namespace MyFactory.Application.Features.Reports.CloseMonthlyFinancialReport;

public sealed class CloseMonthlyFinancialReportCommandValidator
    : AbstractValidator<CloseMonthlyFinancialReportCommand>
{
    public CloseMonthlyFinancialReportCommandValidator()
    {
        RuleFor(x => x.Year)
            .InclusiveBetween(2000, 2100);

        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12);
    }
}
