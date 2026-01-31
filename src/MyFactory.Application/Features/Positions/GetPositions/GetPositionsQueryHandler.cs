using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Positions;

namespace MyFactory.Application.Features.Positions.GetPositions;

public sealed class GetPositionsQueryHandler
    : IRequestHandler<GetPositionsQuery, IReadOnlyList<PositionListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetPositionsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<PositionListItemDto>> Handle(
        GetPositionsQuery request,
        CancellationToken cancellationToken)
    {
        var query =
            from p in _db.Positions.AsNoTracking()
            from dp in p.DepartmentPositions.DefaultIfEmpty()
            join d in _db.Departments.AsNoTracking()
                on dp.DepartmentId equals d.Id into dJoin
            from d in dJoin.DefaultIfEmpty()
            select new PositionListItemDto
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                DepartmentId = d != null ? d.Id : Guid.Empty,
                DepartmentName = d != null ? d.Name : string.Empty,
                IsActive = p.IsActive
            };

        // Filter by IsActive if IncludeInactive is false
        if (!request.IncludeInactive)
        {
            query = query.Where(x => x.IsActive);
        }

        // Optionally filter by DepartmentId if provided
        if (request.DepartmentId.HasValue)
        {
            query = query.Where(x => x.DepartmentId == request.DepartmentId.Value);
        }

        query = request.SortBy switch
        {
            PositionSortBy.Code =>
                request.SortDesc
                    ? query.OrderByDescending(x => x.Code ?? "")
                    : query.OrderBy(x => x.Code ?? ""),

            _ =>
                request.SortDesc
                    ? query.OrderByDescending(x => x.Name)
                    : query.OrderBy(x => x.Name)
        };

        return await query.ToListAsync(cancellationToken);
    }
}
