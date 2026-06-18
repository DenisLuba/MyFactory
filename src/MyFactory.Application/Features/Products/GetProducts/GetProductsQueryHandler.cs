using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Common;
using MyFactory.Application.DTOs.Products;

namespace MyFactory.Application.Features.Products.GetProducts;

public sealed class GetProductsQueryHandler(IApplicationDbContext db)
        : IRequestHandler<GetProductsQuery, ListDto<ProductListItemDto>>
{
    public async Task<ListDto<ProductListItemDto>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var skip = Math.Max(0, request.Skip);
        var take = Math.Clamp(request.Take, 1, 100); 
        var sortBy = request.SortBy?.Trim().ToLowerInvariant();

        var productsQuery = db.Products.AsNoTracking();

        // фильтрация по имени
        if (!string.IsNullOrWhiteSpace(request.SearchName))
        {
            var searchName = request.SearchName.Trim();
            productsQuery = productsQuery
                .Where(p => EF.Functions.ILike(p.Name, $"%{searchName}%"));
        }
        // фильтрация по типу
        if (!string.IsNullOrWhiteSpace(request.SearchType))
        {
            var searchType = request.SearchType.Trim();
            productsQuery = productsQuery
                .Where(p => 
                    p.ProductType != null 
                    && EF.Functions.ILike(p.ProductType.Type, $"%{searchType}%"));
        }

        var totalCount = await productsQuery.CountAsync(cancellationToken);
        if (totalCount == 0)
        {
            return new ListDto<ProductListItemDto>
            {
                Items = [],
                TotalCount = 0,
                Skip = skip,
                Take = take,
                HasMore = false
            };
        }

        if (sortBy is "name" or "type" or null or "")
        {
            var orderedQuery = sortBy switch
            {
                "type" => request.SortDesc
                    ? productsQuery.OrderByDescending(p => p.ProductType != null ? p.ProductType.Type : string.Empty)
                        .ThenByDescending(p => p.Id)
                    : productsQuery.OrderBy(p => p.ProductType != null ? p.ProductType.Type : string.Empty)
                        .ThenBy(p => p.Id),

                _ => request.SortDesc
                    ? productsQuery.OrderByDescending(p => p.Name).ThenByDescending(p => p.Id)
                    : productsQuery.OrderBy(p => p.Name).ThenBy(p => p.Id)
            };

            var productsPage = await orderedQuery
                .Skip(skip)
                .Take(take)
                .Include(p => p.ProductType)
                .Include(p => p.ProductMaterials)
                .Include(p => p.ProductDepartmentCosts)
                .ToListAsync(cancellationToken);

            var materialIds = productsPage
                .SelectMany(p => p.ProductMaterials)
                .Select(pm => pm.MaterialId)
                .Distinct()
                .ToList();
            
            var latestUnitPriceByMaterial = await db.MaterialPurchaseOrderItems
                .AsNoTracking()
                .Where(i => materialIds.Contains(i.MaterialId))
                .GroupBy(i => i.MaterialId)
                .Select(g => new
                {
                    MaterialId = g.Key,
                    UnitPrice = g
                        .OrderByDescending(x => x.CreatedAt)
                        .ThenByDescending(x => x.Id)
                        .Select(x => x.UnitPrice)
                        .FirstOrDefault()
                })
                .ToDictionaryAsync(x => x.MaterialId, x => x.UnitPrice, cancellationToken);

            var items = new List<ProductListItemDto>(productsPage.Count);

            foreach (var product in productsPage)
            {
                decimal materialCost = 0m;
                foreach (var pm in product.ProductMaterials)
                {
                    if (latestUnitPriceByMaterial.TryGetValue(pm.MaterialId, out var unitPrice))
                        materialCost += pm.QtyPerUnit * unitPrice;
                }

                var departmentCost = product.ProductDepartmentCosts
                    .Select(c => c.ExpensesPerUnit + c.CutCostPerUnit + c.SewingCostPerUnit + c.PackCostPerUnit)
                    .DefaultIfEmpty(0m)
                    .Max();

                items.Add(new ProductListItemDto
                {
                    Id = product.Id,
                    Sku = product.Sku,
                    Name = product.Name,
                    ProductTypeId = product.ProductTypeId,
                    ProductTypeName = product.ProductType?.Type,
                    Status = product.Status,
                    Description = product.Description,
                    PlanPerHour = product.PlanPerHour,
                    Version = product.Version,
                    CostPrice = materialCost + departmentCost
                });
            }

            return new ListDto<ProductListItemDto>
            {
                Items = items,
                TotalCount = totalCount,
                Skip = skip,
                Take = take,
                HasMore = skip + items.Count < totalCount
            };
        }

        // Для cost: пока считаем в памяти -> требуется полный набор после фильтра.
        var allProducts = await productsQuery
            .Include(p => p.ProductType)
            .Include(p => p.ProductMaterials)
            .Include(p => p.ProductDepartmentCosts)
            .ToListAsync(cancellationToken);

        var allMaterialIds = allProducts
            .SelectMany(p => p.ProductMaterials)
            .Select(pm => pm.MaterialId)
            .Distinct()
            .ToList();

        var allLatestUnitPriceByMaterial = await db.MaterialPurchaseOrderItems
            .AsNoTracking()
            .Where(i => allMaterialIds.Contains(i.MaterialId))
            .GroupBy(i => i.MaterialId)
            .Select(g => new
            {
                MaterialId = g.Key,
                UnitPrice = g
                    .OrderByDescending(x => x.CreatedAt)
                    .ThenByDescending(x => x.Id)
                    .Select(x => x.UnitPrice)
                    .FirstOrDefault()
            })
            .ToDictionaryAsync(x => x.MaterialId, x => x.UnitPrice, cancellationToken);

        var allItems = new List<ProductListItemDto>(allProducts.Count);

        foreach (var product in allProducts)
        {
            decimal materialCost = 0m;
            foreach (var pm in product.ProductMaterials)
            {
                if (allLatestUnitPriceByMaterial.TryGetValue(pm.MaterialId, out var unitPrice))
                    materialCost += pm.QtyPerUnit * unitPrice;
            }

            var departmentCost = product.ProductDepartmentCosts
                .Select(c => c.ExpensesPerUnit + c.CutCostPerUnit + c.SewingCostPerUnit + c.PackCostPerUnit)
                .DefaultIfEmpty(0m)
                .Max();

            allItems.Add(new ProductListItemDto
            {
                Id = product.Id,
                Sku = product.Sku,
                Name = product.Name,
                ProductTypeId = product.ProductTypeId,
                ProductTypeName = product.ProductType?.Type,
                Status = product.Status,
                Description = product.Description,
                PlanPerHour = product.PlanPerHour,
                Version = product.Version,
                CostPrice = materialCost + departmentCost
            });
        }

        allItems = request.SortDesc
            ? allItems.OrderByDescending(x => x.CostPrice).ThenByDescending(x => x.Id).ToList()
            : allItems.OrderBy(x => x.CostPrice).ThenBy(x => x.Id).ToList();

        var pagedItems = allItems.Skip(skip).Take(take).ToList();

        return new ListDto<ProductListItemDto>
        {
            Items = pagedItems,
            TotalCount = totalCount,
            Skip = skip,
            Take = take,
            HasMore = skip + pagedItems.Count < totalCount
        };
    }
}
