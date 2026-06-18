using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Common;
using MyFactory.Application.DTOs.Materials;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.Features.Materials.GetMaterials;

public sealed class GetMaterialsQueryHandler(IApplicationDbContext db) : IRequestHandler<GetMaterialsQuery, ListDto<MaterialListItemDto>>
{
    private readonly IApplicationDbContext _db = db;

    public async Task<ListDto<MaterialListItemDto>> Handle(GetMaterialsQuery request, CancellationToken cancellationToken)
    {
        var skip = Math.Max(0, request.Skip);
        var take = Math.Clamp(request.Take, 1, 100);
        var sortBy = request.SortBy?.Trim().ToLowerInvariant();

        var materialsQuery = _db.Materials.AsNoTracking().Where(m => m.IsActive == request.IsActive);

        var warehouseMaterialsQuery = _db.WarehouseMaterials.AsNoTracking();

        if (request.WarehouseId.HasValue)
        {
            var warehouseId = request.WarehouseId.Value;
            warehouseMaterialsQuery = warehouseMaterialsQuery.Where(wm => wm.WarehouseId == warehouseId);

            materialsQuery = materialsQuery.Where(m => warehouseMaterialsQuery.Any(wm => wm.MaterialId == m.Id));
        }

        // фильтрация по имени
        if (!string.IsNullOrWhiteSpace(request.SearchName))
        {
            var searchName = request.SearchName;
            materialsQuery = materialsQuery
                .Where(m => EF.Functions.ILike(m.Name, $"%{searchName}%"));
        }
        // фильтрация по типу
        if (!string.IsNullOrWhiteSpace(request.SearchType))
        {
            var searchType = request.SearchType;
            materialsQuery = materialsQuery
                .Where(m =>
                    m.MaterialType != null
                    && EF.Functions.ILike(m.MaterialType.Name, $"%{searchType}%"));
        }

        var totalCount = await materialsQuery.CountAsync(cancellationToken);
        if (totalCount == 0)
        {
            return new ListDto<MaterialListItemDto>
            {
                Items = [],
                TotalCount = 0,
                Skip = skip,
                Take = take,
                HasMore = false
            };
        }

        var items = new List<MaterialListItemDto>();

        if (sortBy is "type" or "name" or null or "")
        {
            // сортировка по типу или названию 
            var orderedQuery = sortBy switch
            {
                "type" => request.SortDesc
                    ? materialsQuery.OrderByDescending(m => m.MaterialType != null ? m.MaterialType.Name : string.Empty)
                        .ThenByDescending(m => m.Id)
                    : materialsQuery.OrderBy(m => m.MaterialType != null ? m.MaterialType.Name : string.Empty)
                        .ThenBy(m => m.Id),

                _ => request.SortDesc
                    ? materialsQuery.OrderByDescending(m => m.Name).ThenByDescending(m => m.Id)
                    : materialsQuery.OrderBy(m => m.Name).ThenBy(m => m.Id),
            };

            // берем выборку по заданному размеру 
            var materialsPage = orderedQuery
                .Skip(skip)
                .Take(take);

            items = await GetItems(materialsPage, warehouseMaterialsQuery).ToListAsync(cancellationToken);  
        }
        else
        {
            var allDtos = GetItems(materialsQuery, warehouseMaterialsQuery);

            // сортируем список и обрезаем его по заданному размеру (- skip + take)
            items = request.SortDesc 
                ? await allDtos
                    .OrderByDescending(dto => dto.TotalQty)
                    .ThenByDescending(dto => dto.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync(cancellationToken)
                : await allDtos
                    .OrderBy(dto => dto.TotalQty)
                    .ThenBy(dto => dto.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync(cancellationToken);
        }

        return new ListDto<MaterialListItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            Take = take,
            Skip = skip,
            HasMore = skip + items.Count < totalCount
        };
    }

    private IQueryable<MaterialListItemDto> GetItems(
        IQueryable<MaterialEntity> materials,
        IQueryable<WarehouseMaterialEntity> warehouseMaterials) =>
        from m in materials
        join t in _db.MaterialTypes.AsNoTracking() on m.MaterialTypeId equals t.Id into tGroup
        from t in tGroup.DefaultIfEmpty()
        join u in _db.Units.AsNoTracking() on m.UnitId equals u.Id into uGroup
        from u in uGroup.DefaultIfEmpty()
        join wm in warehouseMaterials on m.Id equals wm.MaterialId into wmGroup
        select new MaterialListItemDto
        {
            Id = m.Id,
            MaterialType = t.Name,
            Name = m.Name,
            UnitCode = u.Code,
            TotalQty = wmGroup.Sum(wm => wm.Qty)
        };
}
