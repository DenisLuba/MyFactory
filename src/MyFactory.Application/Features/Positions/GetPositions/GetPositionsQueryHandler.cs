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
            join d in _db.Departments.AsNoTracking()
                on p.DepartmentId equals d.Id
            select new PositionListItemDto
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                DepartmentName = d.Name,
                IsActive = p.IsActive
            };

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
