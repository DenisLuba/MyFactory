namespace MyFactory.WebApi.Contracts.PayrollRules;

public record PayrollRuleResponse(
    Guid Id,
    DateOnly EffectiveFrom,
    decimal PremiumPercent,
    string Description);
