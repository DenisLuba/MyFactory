using MediatR;
using MyFactory.Application.DTOs.Common;
using MyFactory.Application.DTOs.Products;

namespace MyFactory.Application.Features.Products.GetProducts;

public sealed record GetProductsQuery(
    string? SearchName = null,
    string? SearchType = null,
    string? SortBy = null,     
    bool SortDesc = false,
    int Skip = 0,
    int Take = 30    
) : IRequest<ListDto<ProductListItemDto>>;

