using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Payroll;

namespace MyFactory.Application.OldFeatures.Payroll.Queries.GetPayrollForPeriod;

public sealed class GetPayrollForPeriodQueryHandler : IRequestHandler<GetPayrollForPeriodQuery, IReadOnlyCollection<PayrollEntryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPayrollForPeriodQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<PayrollEntryDto>> Handle(GetPayrollForPeriodQuery request, CancellationToken cancellationToken)
    {
        var payrollEntries = await _context.PayrollEntries
            .AsNoTracking()
            .Where(entry => entry.PeriodStart >= request.FromDate && entry.PeriodEnd <= request.ToDate)
            .OrderBy(entry => entry.EmployeeId)
            .ThenBy(entry => entry.PeriodStart)
            .ToListAsync(cancellationToken);

        if (payrollEntries.Count == 0)
        {
            return Array.Empty<PayrollEntryDto>();
        }

        var employeeIds = payrollEntries
            .Select(entry => entry.EmployeeId)
            .Distinct()
            .ToList();

        var employees = await _context.Employees
            .AsNoTracking()
            .Where(employee => employeeIds.Contains(employee.Id))
            .ToDictionaryAsync(employee => employee.Id, cancellationToken);

        return payrollEntries
            .Select(entry => PayrollEntryDto.FromEntity(entry, employees.TryGetValue(entry.EmployeeId, out var employee)
                ? employee.FullName
                : string.Empty))
            .ToList();
    }
}
