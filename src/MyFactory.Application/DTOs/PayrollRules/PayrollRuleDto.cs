namespace MyFactory.Application.DTOs.PayrollRules;

public sealed record PayrollRuleDto
(
    Guid Id, 
    string Name,
    DateOnly EffectiveFrom, 
    decimal PremiumPercent, 
    string Description
);


