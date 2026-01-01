using MediatR;

namespace MyFactory.Application.Features.PayrollRules.CreatePayrollRule;

public sealed record CreatePayrollRuleCommand(DateOnly EffectiveFrom, decimal PremiumPercent, string Description) : IRequest<Guid>;
