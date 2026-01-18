using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Products;

namespace MyFactory.Application.Features.Products.GetProducts;

public sealed class GetProductsQueryHandler
    : IRequestHandler<GetProductsQuery, IReadOnlyList<ProductListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetProductsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ProductListItemDto>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var productsQuery = _db.Products
            .AsNoTracking()
            .Where(p => p.Status == Domain.Entities.Products.ProductStatus.Active);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            productsQuery = productsQuery
                .Where(p => p.Name.Contains(request.Search));
        }

        var products = await productsQuery
            .Include(p => p.ProductMaterials)
            .Include(p => p.ProductDepartmentCosts)
            .ToListAsync(cancellationToken);

        var result = new List<ProductListItemDto>();

        foreach (var product in products)
        {
            // 1. Материальная себестоимость
            decimal materialCost = 0m;

            foreach (var pm in product.ProductMaterials)
            {
                // последняя цена закупки материала
                var lastPrice = await _db.MaterialPurchaseOrderItems
                    .AsNoTracking()
                    .Where(i => i.MaterialId == pm.MaterialId)
                    .OrderByDescending(i => i.CreatedAt)
                    .Select(i => i.UnitPrice)
                    .FirstOrDefaultAsync(cancellationToken);

                materialCost += pm.QtyPerUnit * lastPrice;
            }

            // 2. Цеховые расходы
            decimal departmentCost = product.ProductDepartmentCosts.Sum(c =>
                c.ExpensesPerUnit +
                c.CutCostPerUnit +
                c.SewingCostPerUnit +
                c.PackCostPerUnit
            );

            result.Add(new ProductListItemDto
            {
                Id = product.Id,
                Name = product.Name,
                Sku = product.Sku,
                Status = product.Status,
                Description = product.Description,
                PlanPerHour = product.PlanPerHour,
                Version = product.Version,
                CostPrice = materialCost + departmentCost
            });
        }

        result = request.SortBy switch
        {
            "cost" => request.SortDesc
                ? result.OrderByDescending(x => x.CostPrice).ToList()
                : result.OrderBy(x => x.CostPrice).ToList(),

            _ => request.SortDesc
                ? result.OrderByDescending(x => x.Name).ToList()
                : result.OrderBy(x => x.Name).ToList()
        };

        return result;
    }
}
