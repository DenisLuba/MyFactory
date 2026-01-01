using MediatR;

namespace MyFactory.Application.Features.PayrollRules.DeletePayrollRule;

public sealed record DeletePayrollRuleCommand(Guid Id) : IRequest;
