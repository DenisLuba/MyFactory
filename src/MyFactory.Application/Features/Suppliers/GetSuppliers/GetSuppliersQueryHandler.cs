using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.DTOs.Common;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Suppliers;

namespace MyFactory.Application.Features.Suppliers.GetSuppliers;

public sealed class GetSuppliersQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetSuppliersQuery, ListDto<SupplierListItemDto>>
{
    public async Task<ListDto<SupplierListItemDto>> Handle(
        GetSuppliersQuery request,
        CancellationToken cancellationToken)
    {
        var skip = Math.Max(0, request.Skip);
        var take = Math.Clamp(request.Take, 1, 100);
        var sortBy = request.SortBy?.Trim().ToLowerInvariant();

        var suppliersQuery = db.Suppliers.AsNoTracking();

        if (request.IsActive.HasValue)
        {
            suppliersQuery = suppliersQuery.Where(x => x.IsActive == request.IsActive.Value);
        }

        if (request.SearchName is string searchName)
        {
            suppliersQuery = suppliersQuery
                .Where(c => EF.Functions.ILike(c.Name, $"%{searchName}%"));
        }

        var totalCount = await suppliersQuery.CountAsync(cancellationToken);
        if (totalCount == 0)
        {
            return new ListDto<SupplierListItemDto>
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
            "name" => request.SortDesc
                ? suppliersQuery.OrderByDescending(c => c.Name).ThenByDescending(c => c.Id)
                : suppliersQuery.OrderBy(c => c.Name).ThenBy(c => c.Id),

            _ => request.SortDesc
                ? suppliersQuery.OrderByDescending(c => c.CreatedAt).ThenByDescending(c => c.Id)
                : suppliersQuery.OrderBy(c => c.CreatedAt).ThenBy(c => c.Id)
        };

        var items = await orderedQuery
            .Skip(skip)
            .Take(take)
            .Select(s => new SupplierListItemDto
            {
                Id = s.Id,
                Name = s.Name,
                IsActive = s.IsActive 
            })
            .ToListAsync();

        return new ListDto<SupplierListItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            Skip = skip,
            Take = take,
            HasMore = skip + items.Count < totalCount
        };
    }
}
