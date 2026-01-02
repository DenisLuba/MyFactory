using FluentValidation;

namespace MyFactory.Application.Features.Reports.GetMonthlyFinancialReports;

public sealed class GetMonthlyFinancialReportsQueryValidator
    : AbstractValidator<GetMonthlyFinancialReportsQuery>
{
    public GetMonthlyFinancialReportsQueryValidator()
    {
        // No parameters to validate.
    }
}
