namespace MyFactory.WebApi.Contracts.PayrollRules;

public record UpdatePayrollRuleRequest(
    Guid Id,
    string Name,
    DateOnly EffectiveFrom,
    decimal PremiumPercent,
    string Description);
