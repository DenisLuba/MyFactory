namespace MyFactory.WebApi.Contracts.PayrollRules;

public record UpdatePayrollRuleRequest(
    DateOnly EffectiveFrom,
    decimal PremiumPercent,
    string Description);
