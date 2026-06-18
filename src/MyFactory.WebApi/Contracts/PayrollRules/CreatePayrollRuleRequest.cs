namespace MyFactory.WebApi.Contracts.PayrollRules;

public record CreatePayrollRuleRequest(
    string Name,
    DateOnly EffectiveFrom,
    decimal PremiumPercent,
    string Description);
