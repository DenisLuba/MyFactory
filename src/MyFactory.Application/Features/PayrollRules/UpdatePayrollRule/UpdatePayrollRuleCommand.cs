using MediatR;

namespace MyFactory.Application.Features.PayrollRules.UpdatePayrollRule;

public sealed record UpdatePayrollRuleCommand(string Name, Guid Id, DateOnly EffectiveFrom, decimal PremiumPercent, string Description) : IRequest;
