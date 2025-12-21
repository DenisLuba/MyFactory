using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Payroll;

namespace MyFactory.Application.OldFeatures.Payroll.Queries.GetTimesheetEntriesByEmployee;

public sealed class GetTimesheetEntriesByEmployeeQueryHandler : IRequestHandler<GetTimesheetEntriesByEmployeeQuery, IReadOnlyCollection<TimesheetEntryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetTimesheetEntriesByEmployeeQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<TimesheetEntryDto>> Handle(GetTimesheetEntriesByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == request.EmployeeId, cancellationToken);

        if (employee is null)
        {
            throw new InvalidOperationException("Employee not found.");
        }

        var query = _context.TimesheetEntries
            .AsNoTracking()
            .Where(entry => entry.EmployeeId == request.EmployeeId);

        if (request.FromDate.HasValue)
        {
            query = query.Where(entry => entry.WorkDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(entry => entry.WorkDate <= request.ToDate.Value);
        }

        var entries = await query
            .OrderBy(entry => entry.WorkDate)
            .ThenBy(entry => entry.Id)
            .ToListAsync(cancellationToken);

        return entries
            .Select(entry => TimesheetEntryDto.FromEntity(entry, employee.FullName))
            .ToList();
    }
}
