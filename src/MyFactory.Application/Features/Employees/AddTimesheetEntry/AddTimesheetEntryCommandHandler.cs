using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.Features.Employees.AddTimesheetEntry;

public sealed class AddTimesheetEntryCommandHandler
    : IRequestHandler<AddTimesheetEntryCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public AddTimesheetEntryCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(
        AddTimesheetEntryCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await _db.Employees
            .FirstOrDefaultAsync(x => x.Id == request.EmployeeId, cancellationToken)
            ?? throw new NotFoundException("Employee not found");

        if (!employee.IsActive)
            throw new DomainException("Cannot add timesheet entry for inactive employee.");

        var position = await _db.Positions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == employee.PositionId, cancellationToken)
            ?? throw new NotFoundException("Position not found");

        var exists = await _db.Timesheets.AnyAsync(
            x => x.EmployeeId == request.EmployeeId
              && x.WorkDate == request.Date,
            cancellationToken);

        if (exists)
            throw new DomainException("Timesheet entry for this date already exists.");

        var entry = new TimesheetEntity(
            employee.Id,
            position.DepartmentId,
            request.Date,
            request.Hours,
            request.Comment);

        _db.Timesheets.Add(entry);
        await _db.SaveChangesAsync(cancellationToken);

        return entry.Id;
    }
}
