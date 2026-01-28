using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Employees;

namespace MyFactory.Application.Features.Organization.Employees.GetEmployee;

public sealed class GetEmployeesQueryHandler
    : IRequestHandler<GetEmployeesQuery, IReadOnlyList<EmployeeListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetEmployeesQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<EmployeeListItemDto>> Handle(
        GetEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        var query =
            from e in _db.Employees.AsNoTracking()
            join p in _db.Positions.AsNoTracking() on e.PositionId equals p.Id
            join d in _db.Departments.AsNoTracking() on e.DepartmentId equals d.Id
            select new
            {
                e,
                PositionName = p.Name,
                DepartmentName = d.Name
            };

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            // query = query.Where(x => x.e.FullName.Contains(search));
            query = query.Where(x => EF.Functions.Like(x.e.FullName, $"%{search}%"));
        }

        if (!request.IncludeInactive)
        {
            query = query.Where(x => x.e.IsActive);
        }

        query = request.SortBy switch
        {
            EmployeeSortBy.Department => request.SortDesc
                ? query.OrderByDescending(x => x.DepartmentName)
                : query.OrderBy(x => x.DepartmentName),

            _ => request.SortDesc
                ? query.OrderByDescending(x => x.e.FullName)
                : query.OrderBy(x => x.e.FullName)
        };

        return await query
            .Select(x => new EmployeeListItemDto
            {
                Id = x.e.Id,
                FullName = x.e.FullName,
                DepartmentName = x.DepartmentName,
                PositionName = x.PositionName,
                IsActive = x.e.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}
