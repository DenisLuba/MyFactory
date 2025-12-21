using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Payroll;
using MyFactory.Domain.Entities.Employees;

namespace MyFactory.Application.OldFeatures.Payroll.Commands.ClosePayrollPeriod;

public sealed class ClosePayrollPeriodCommandHandler : IRequestHandler<ClosePayrollPeriodCommand, PayrollSummaryDto>
{
    private readonly IApplicationDbContext _context;

    public ClosePayrollPeriodCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PayrollSummaryDto> Handle(ClosePayrollPeriodCommand request, CancellationToken cancellationToken)
    {
        var overlappingPeriodExists = await _context.PayrollEntries
            .AsNoTracking()
            .AnyAsync(entry => entry.PeriodStart <= request.ToDate && entry.PeriodEnd >= request.FromDate, cancellationToken);

        if (overlappingPeriodExists)
        {
            throw new InvalidOperationException("Payroll period already closed.");
        }

        var timesheetEntries = await _context.TimesheetEntries
            .AsNoTracking()
            .Where(entry => entry.WorkDate >= request.FromDate && entry.WorkDate <= request.ToDate)
            .ToListAsync(cancellationToken);

        if (timesheetEntries.Count == 0)
        {
            return PayrollSummaryDto.Empty(request.FromDate, request.ToDate);
        }

        var employeeIds = timesheetEntries
            .Select(entry => entry.EmployeeId)
            .Distinct()
            .ToList();

        var employees = await _context.Employees
            .AsNoTracking()
            .Where(employee => employeeIds.Contains(employee.Id))
            .ToDictionaryAsync(employee => employee.Id, cancellationToken);

        var payrollEntries = new List<PayrollEntry>();

        foreach (var grouping in timesheetEntries.GroupBy(entry => entry.EmployeeId))
        {
            if (!employees.TryGetValue(grouping.Key, out var employee))
            {
                throw new InvalidOperationException("Employee not found for payroll calculation.");
            }

            var totalHours = grouping.Sum(entry => entry.HoursWorked);
            var premiumFactor = 1 + (employee.PremiumPercent / 100m);
            var accruedAmount = decimal.Round(totalHours * employee.RatePerNormHour * premiumFactor, 2, MidpointRounding.AwayFromZero);

            payrollEntries.Add(PayrollEntry.Create(employee.Id, request.FromDate, request.ToDate, totalHours, accruedAmount));
        }

        await _context.PayrollEntries.AddRangeAsync(payrollEntries, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var payrollDtos = payrollEntries
            .Select(entry =>
                PayrollEntryDto.FromEntity(entry, employees[entry.EmployeeId].FullName))
            .ToList();

        return PayrollSummaryDto.FromEntries(request.FromDate, request.ToDate, payrollDtos);
    }
}
