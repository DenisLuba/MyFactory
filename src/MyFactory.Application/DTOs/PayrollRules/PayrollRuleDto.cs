namespace MyFactory.Application.DTOs.PayrollRules;

public sealed record PayrollRuleDto
(
    Guid Id, 
    DateOnly EffectiveFrom, 
    decimal PremiumPercent, 
    string Description
);


