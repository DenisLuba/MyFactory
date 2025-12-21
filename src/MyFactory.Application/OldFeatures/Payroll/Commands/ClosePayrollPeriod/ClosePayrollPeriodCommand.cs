using System;
using MediatR;
using MyFactory.Application.DTOs.Payroll;

namespace MyFactory.Application.OldFeatures.Payroll.Commands.ClosePayrollPeriod;

public sealed record ClosePayrollPeriodCommand(DateOnly FromDate, DateOnly ToDate) : IRequest<PayrollSummaryDto>;
