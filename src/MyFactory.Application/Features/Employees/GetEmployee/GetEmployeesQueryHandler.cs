using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Common;
using MyFactory.Application.DTOs.Employees;

namespace MyFactory.Application.Features.Employees.GetEmployee;

public sealed class GetEmployeesQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetEmployeesQuery, ListDto<EmployeeListItemDto>>
{
    public async Task<ListDto<EmployeeListItemDto>> Handle(
        GetEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        var skip = Math.Max(0, request.Skip);
        var take = Math.Clamp(request.Take, 1, 100);
        var sortBy = request.SortBy?.Trim().ToLowerInvariant();

        IQueryable<Domain.Entities.Organization.EmployeeEntity> employeesQuery = db.Employees.AsNoTracking()
            .Include(e => e.Position);

        if (request.IsActive.HasValue)
        {
            employeesQuery = employeesQuery.Where(e => e.IsActive == request.IsActive.Value);
        }

        if (request.DepartmentId.HasValue)
        {
            employeesQuery = employeesQuery.Where(e => e.DepartmentId == request.DepartmentId.Value);
        }

        if (request.PositionId.HasValue)
        {
            employeesQuery = employeesQuery.Where(e => e.PositionId == request.PositionId.Value);
        }

        if (request.Grade.HasValue)
        {
            employeesQuery = employeesQuery.Where(e => e.Grade == request.Grade.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.FullName))
        {
            var fullName = request.FullName.Trim();
            employeesQuery = employeesQuery.Where(e => EF.Functions.ILike(e.FullName, $"%{fullName}%"));
        }

        if (request.HiredFrom.HasValue)
        {
            var hiredFrom = Normalize(request.HiredFrom.Value);
            employeesQuery = employeesQuery.Where(e => e.HiredAt >= hiredFrom);
        }

        if (request.HiredTo.HasValue)
        {
            var hiredTo = Normalize(request.HiredTo.Value);
            employeesQuery = employeesQuery.Where(e => e.HiredAt <= hiredTo);
        }

        if (request.CanCut.HasValue)
        {
            employeesQuery = employeesQuery.Where(e => e.Position != null && e.Position.CanCut == request.CanCut.Value);
        }

        if (request.CanSew.HasValue)
        {
            employeesQuery = employeesQuery.Where(e => e.Position != null && e.Position.CanSew == request.CanSew.Value);
        }

        if (request.CanPackage.HasValue)
        {
            employeesQuery = employeesQuery.Where(e => e.Position != null && e.Position.CanPackage == request.CanPackage.Value);
        }

        if (request.ExceptEmployeeIds != null && request.ExceptEmployeeIds.Count > 0)
        {
            employeesQuery = employeesQuery.Where(e => !request.ExceptEmployeeIds.Contains(e.Id));
        }

        var totalCount = await employeesQuery.CountAsync(cancellationToken);
        if (totalCount == 0)
        {
            return new ListDto<EmployeeListItemDto>
            {
                Items = [],
                TotalCount = 0,
                Skip = skip,
                Take = take,
                HasMore = false
            };
        }

        var orderedQuery = sortBy switch
        {
            "fullname" => request.SortDesc
                ? employeesQuery.OrderByDescending(e => e.FullName).ThenByDescending(e => e.Id)
                : employeesQuery.OrderBy(e => e.FullName).ThenBy(e => e.Id),

            "department" => request.SortDesc
                ? employeesQuery
                    .OrderByDescending(e => e.Department != null ? e.Department.Name : string.Empty)
                    .ThenBy(e => e.FullName)
                    .ThenByDescending(e => e.Id)
                : employeesQuery
                    .OrderBy(e => e.Department != null ? e.Department.Name : string.Empty)
                    .ThenBy(e => e.FullName)
                    .ThenByDescending(e => e.Id),

            "position" => request.SortDesc
                ? employeesQuery
                    .OrderByDescending(e => e.Position != null ? e.Position.Name : string.Empty)
                    .ThenBy(e => e.FullName)
                    .ThenByDescending(e => e.Id)
                : employeesQuery
                    .OrderBy(e => e.Position != null ? e.Position.Name : string.Empty)
                    .ThenBy(e => e.FullName)
                    .ThenByDescending(e => e.Id),

            _ => request.SortDesc
                ? employeesQuery.OrderByDescending(e => e.FullName).ThenByDescending(e => e.Id)
                : employeesQuery.OrderBy(e => e.FullName).ThenBy(e => e.Id)
        };

        var items = await orderedQuery
            .Include(e => e.Department)
            .Include(e => e.Position)
            .Skip(skip)
            .Take(take)
            .Select(e => new EmployeeListItemDto
            {
                Id = e.Id,
                FullName = e.FullName,
                DepartmentName = e.Department != null ? e.Department.Name : string.Empty,
                PositionName = e.Position != null ? e.Position.Name : string.Empty,
                IsActive = e.IsActive
            })
            .ToListAsync(cancellationToken);

        return new ListDto<EmployeeListItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            Skip = skip,
            Take = take,
            HasMore = skip + items.Count < totalCount
        };
    }

    private static DateTime Normalize(DateTime value) =>
        value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
}
