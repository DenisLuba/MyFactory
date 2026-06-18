using FluentValidation;

namespace MyFactory.Application.Features.Reports.ApproveMonthlyFinancialReport;

public sealed class ApproveMonthlyFinancialReportCommandValidator
    : AbstractValidator<ApproveMonthlyFinancialReportCommand>
{
    public ApproveMonthlyFinancialReportCommandValidator()
    {
        RuleFor(x => x.Year)
            .InclusiveBetween(2000, 2100);

        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12);
    }
}
