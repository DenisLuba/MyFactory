using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.Features.Materials.GetMaterials;

public sealed class GetMaterialsQueryHandler : IRequestHandler<GetMaterialsQuery, IReadOnlyList<MaterialListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetMaterialsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<MaterialListItemDto>> Handle(GetMaterialsQuery request, CancellationToken cancellationToken)
    {
        var query =
            from m in _db.Materials.AsNoTracking()
            join t in _db.MaterialTypes.AsNoTracking() on m.MaterialTypeId equals t.Id
            join u in _db.Units.AsNoTracking() on m.UnitId equals u.Id
            join wm in _db.WarehouseMaterials.AsNoTracking() on m.Id equals wm.MaterialId into wmGroup
            from wm in wmGroup.DefaultIfEmpty()
            select new { m, t, u, wm };

        if (request.Filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(request.Filter.MaterialName))
                query = query.Where(x =>
                    EF.Functions.Like(x.m.Name, $"%{request.Filter.MaterialName}%"));

            if (!string.IsNullOrWhiteSpace(request.Filter.MaterialType))
                query = query.Where(x =>
                    EF.Functions.Like(x.t.Name, $"%{request.Filter.MaterialType}%"));

            if (request.Filter.IsActive.HasValue)
                query = query.Where(x =>
                    x.m.IsActive == request.Filter.IsActive.Value);

            if (request.Filter.WarehouseId.HasValue)
                query = query.Where(x =>
                    x.wm != null && x.wm.WarehouseId == request.Filter.WarehouseId.Value);
        }

        var grouped = query
            .GroupBy(x => new
            {
                MaterialId = x.m.Id,
                MaterialName = x.m.Name,
                MaterialTypeName = x.t.Name,
                UnitCode = x.u.Code
            })
            .Select(g => new MaterialListItemDto
            {
                Id = g.Key.MaterialId,
                Name = g.Key.MaterialName,
                MaterialType = g.Key.MaterialTypeName,
                UnitCode = g.Key.UnitCode,
                TotalQty = g.Sum(x => x.wm != null ? x.wm.Qty : 0)
            });

        return await grouped.ToListAsync(cancellationToken);
    }
}
