using MediatR;
using MyFactory.Application.DTOs.PayrollRules;

namespace MyFactory.Application.Features.PayrollRules.GetPayrollRules;

public sealed record GetPayrollRulesQuery : IRequest<IReadOnlyList<PayrollRuleDto>>;
