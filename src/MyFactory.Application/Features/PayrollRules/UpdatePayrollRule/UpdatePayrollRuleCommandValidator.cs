using FluentValidation;

namespace MyFactory.Application.Features.PayrollRules.UpdatePayrollRule;

public sealed class UpdatePayrollRuleCommandValidator : AbstractValidator<UpdatePayrollRuleCommand>
{
    public UpdatePayrollRuleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.EffectiveFrom)
            .Must(d => d != default)
            .WithMessage("EffectiveFrom must be specified.");

        RuleFor(x => x.PremiumPercent)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500);
    }
}
