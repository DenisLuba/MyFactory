using MediatR;
using MyFactory.Application.DTOs.Products;

namespace MyFactory.Application.Features.Products.GetProducts;

public sealed record GetProductsQuery(
    string? Search = null,
    string? SortBy = null,      // "name" | "cost"
    bool SortDesc = false
) : IRequest<IReadOnlyList<ProductListItemDto>>;
