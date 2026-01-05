namespace MyFactory.MauiClient.Models.PayrollRules;

public record CreatePayrollRuleRequest(
    DateOnly EffectiveFrom,
    decimal PremiumPercent,
    string Description);
