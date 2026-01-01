using FluentValidation;

namespace MyFactory.Application.Features.PayrollRules.GetPayrollRules;

public sealed class GetPayrollRulesQueryValidator : AbstractValidator<GetPayrollRulesQuery>
{
    public GetPayrollRulesQueryValidator()
    {
        // No filters yet; validator present for pipeline consistency.
    }
}
