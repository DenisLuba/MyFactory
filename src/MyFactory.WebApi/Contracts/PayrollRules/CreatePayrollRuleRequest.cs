namespace MyFactory.WebApi.Contracts.PayrollRules;

public record CreatePayrollRuleRequest(
    DateOnly EffectiveFrom,
    decimal PremiumPercent,
    string Description);
