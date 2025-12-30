using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Departments;

namespace MyFactory.Application.Features.Departments.GetDepartments;

public sealed class GetDepartmentsQueryHandler
    : IRequestHandler<GetDepartmentsQuery, IReadOnlyList<DepartmentListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetDepartmentsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<DepartmentListItemDto>> Handle(
        GetDepartmentsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _db.Departments.AsNoTracking();

        if (!request.IncludeInactive)
            query = query.Where(x => x.IsActive);

        return await query
            .OrderBy(x => x.Name)
            .Select(x => new DepartmentListItemDto
            {
                Id = x.Id,
                Code = x.Code!,
                Name = x.Name,
                Type = x.Type,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}
