using MediatR;
using MyFactory.Application.DTOs.Suppliers;

namespace MyFactory.Application.Features.Suppliers.GetSuppliers;

public sealed record GetSuppliersQuery(string? Search)
    : IRequest<IReadOnlyList<SupplierListItemDto>>;
