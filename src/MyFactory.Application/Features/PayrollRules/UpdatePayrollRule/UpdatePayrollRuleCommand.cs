using MediatR;

namespace MyFactory.Application.Features.PayrollRules.UpdatePayrollRule;

public sealed record UpdatePayrollRuleCommand(Guid Id, DateOnly EffectiveFrom, decimal PremiumPercent, string Description) : IRequest;
