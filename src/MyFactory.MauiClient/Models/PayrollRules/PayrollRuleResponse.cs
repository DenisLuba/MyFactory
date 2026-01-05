namespace MyFactory.MauiClient.Models.PayrollRules;

public record PayrollRuleResponse(
    Guid Id,
    DateOnly EffectiveFrom,
    decimal PremiumPercent,
    string Description);
