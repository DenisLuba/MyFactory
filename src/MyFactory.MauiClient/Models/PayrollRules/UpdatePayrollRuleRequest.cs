namespace MyFactory.MauiClient.Models.PayrollRules;

public record UpdatePayrollRuleRequest(
    DateOnly EffectiveFrom,
    decimal PremiumPercent,
    string Description);
