using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Payroll;

namespace MyFactory.Application.Features.Payroll.Queries.GetPayrollForPeriod;

public sealed record GetPayrollForPeriodQuery(DateOnly FromDate, DateOnly ToDate) : IRequest<IReadOnlyCollection<PayrollEntryDto>>;
