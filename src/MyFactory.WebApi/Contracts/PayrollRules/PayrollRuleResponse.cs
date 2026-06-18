namespace MyFactory.WebApi.Contracts.PayrollRules;

public record PayrollRuleResponse(
    Guid Id,
    string Name,
    DateOnly EffectiveFrom,
    decimal PremiumPercent,
    string Description);
