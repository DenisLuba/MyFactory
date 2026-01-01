using MediatR;
using MyFactory.Application.DTOs.PayrollRules;

namespace MyFactory.Application.Features.PayrollRules.GetPayrollRuleDetails;

public sealed record GetPayrollRuleDetailsQuery(Guid Id) : IRequest<PayrollRuleDto>;
