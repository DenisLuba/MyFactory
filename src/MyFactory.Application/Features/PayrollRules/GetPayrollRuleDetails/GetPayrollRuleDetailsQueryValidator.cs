using FluentValidation;

namespace MyFactory.Application.Features.PayrollRules.GetPayrollRuleDetails;

public sealed class GetPayrollRuleDetailsQueryValidator : AbstractValidator<GetPayrollRuleDetailsQuery>
{
    public GetPayrollRuleDetailsQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
