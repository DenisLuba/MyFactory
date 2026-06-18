using MediatR;
using MyFactory.Application.DTOs.Common;
using MyFactory.Application.DTOs.Suppliers;

namespace MyFactory.Application.Features.Suppliers.GetSuppliers;

public sealed record GetSuppliersQuery(
    string? SearchName = null,
    string? SortBy = null,
    bool SortDesc = false,
    int Skip = 0,
    int Take = 30,
    bool? IsActive = null   
) : IRequest<ListDto<SupplierListItemDto>>;
