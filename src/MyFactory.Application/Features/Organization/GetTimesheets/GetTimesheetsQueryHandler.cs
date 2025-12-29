using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Organization;

namespace MyFactory.Application.Features.GetTimesheets;

public sealed class GetTimesheetsQueryHandler
    : IRequestHandler<GetTimesheetsQuery, IReadOnlyList<TimesheetListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetTimesheetsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<TimesheetListItemDto>> Handle(
        GetTimesheetsQuery request,
        CancellationToken cancellationToken)
    {
        var query =
            from t in _db.Timesheets.AsNoTracking()
            join e in _db.Employees.AsNoTracking() on t.EmployeeId equals e.Id
            join d in _db.Departments.AsNoTracking() on t.DepartmentId equals d.Id
            where t.WorkDate.Year == request.Period.Year
               && t.WorkDate.Month == request.Period.Month
            select new
            {
                EmployeeId = e.Id,
                e.FullName,
                DepartmentId = d.Id,
                DepartmentName = d.Name,
                t.HoursWorked,
                t.WorkDate
            };

        if (request.EmployeeId.HasValue)
            query = query.Where(x => x.EmployeeId == request.EmployeeId.Value);

        if (request.DepartmentId.HasValue)
            query = query.Where(x => x.DepartmentId == request.DepartmentId.Value);

        return await query
            .GroupBy(x => new { x.EmployeeId, x.FullName, x.DepartmentName })
            .Select(g => new TimesheetListItemDto
            {
                EmployeeId = g.Key.EmployeeId,
                EmployeeName = g.Key.FullName,
                DepartmentName = g.Key.DepartmentName,
                TotalHours = g.Sum(x => x.HoursWorked),
                WorkDays = g.Select(x => x.WorkDate).Distinct().Count()
            })
            .ToListAsync(cancellationToken);
    }
}
