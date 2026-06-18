using FluentValidation;

namespace MyFactory.Application.Features.PayrollRules.DeletePayrollRule;

public sealed class DeletePayrollRuleCommandValidator : AbstractValidator<DeletePayrollRuleCommand>
{
    public DeletePayrollRuleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
