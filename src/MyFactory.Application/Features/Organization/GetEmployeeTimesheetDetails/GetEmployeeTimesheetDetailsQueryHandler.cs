using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Organization;

namespace MyFactory.Application.Features.GetEmployeeTimesheetDetails;

public sealed class GetEmployeeTimesheetDetailsQueryHandler
    : IRequestHandler<GetEmployeeTimesheetDetailsQuery, IReadOnlyList<EmployeeTimesheetEntryDto>>
{
    private readonly IApplicationDbContext _db;

    public GetEmployeeTimesheetDetailsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<EmployeeTimesheetEntryDto>> Handle(
        GetEmployeeTimesheetDetailsQuery request,
        CancellationToken cancellationToken)
    {
        return await _db.Timesheets
            .AsNoTracking()
            .Where(x =>
                x.EmployeeId == request.EmployeeId &&
                x.WorkDate.Year == request.Period.Year &&
                x.WorkDate.Month == request.Period.Month)
            .OrderBy(x => x.WorkDate)
            .Select(x => new EmployeeTimesheetEntryDto
            {
                EntryId = x.Id,
                Date = x.WorkDate,
                Hours = x.HoursWorked
            })
            .ToListAsync(cancellationToken);
    }
}
