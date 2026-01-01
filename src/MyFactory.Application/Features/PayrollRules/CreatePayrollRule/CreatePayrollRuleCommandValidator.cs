using FluentValidation;

namespace MyFactory.Application.Features.PayrollRules.CreatePayrollRule;

public sealed class CreatePayrollRuleCommandValidator : AbstractValidator<CreatePayrollRuleCommand>
{
    public CreatePayrollRuleCommandValidator()
    {
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
