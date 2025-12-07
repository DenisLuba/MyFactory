using System;
using MediatR;
using MyFactory.Application.DTOs.Payroll;

namespace MyFactory.Application.Features.Payroll.Commands.ClosePayrollPeriod;

public sealed record ClosePayrollPeriodCommand(DateOnly FromDate, DateOnly ToDate) : IRequest<PayrollSummaryDto>;
