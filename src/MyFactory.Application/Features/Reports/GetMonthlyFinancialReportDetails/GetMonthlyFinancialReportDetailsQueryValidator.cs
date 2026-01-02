using FluentValidation;

namespace MyFactory.Application.Features.Reports.GetMonthlyFinancialReportDetails;

public sealed class GetMonthlyFinancialReportDetailsQueryValidator
    : AbstractValidator<GetMonthlyFinancialReportDetailsQuery>
{
    public GetMonthlyFinancialReportDetailsQueryValidator()
    {
        RuleFor(x => x.Year)
            .InclusiveBetween(2000, 2100);

        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12);
    }
}
