using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Payroll;
using MyFactory.Domain.Entities.Employees;

namespace MyFactory.Application.Features.Payroll.Commands.RecordTimesheetEntry;

public sealed class RecordTimesheetEntryCommandHandler : IRequestHandler<RecordTimesheetEntryCommand, TimesheetEntryDto>
{
    private readonly IApplicationDbContext _context;

    public RecordTimesheetEntryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TimesheetEntryDto> Handle(RecordTimesheetEntryCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == request.EmployeeId, cancellationToken);

        if (employee is null)
        {
            throw new InvalidOperationException("Employee not found.");
        }

        var entry = TimesheetEntry.Create(request.EmployeeId, request.WorkDate, request.HoursWorked, request.ProductionOrderId);

        await _context.TimesheetEntries.AddAsync(entry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return TimesheetEntryDto.FromEntity(entry, employee.FullName);
    }
}
