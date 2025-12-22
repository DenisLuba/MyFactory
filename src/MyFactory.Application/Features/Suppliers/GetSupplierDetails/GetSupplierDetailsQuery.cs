using MediatR;
using MyFactory.Application.DTOs.Suppliers;

namespace MyFactory.Application.Features.Suppliers.GetSupplierDetails;
public sealed record GetSupplierDetailsQuery(Guid SupplierId)
    : IRequest<SupplierDetailsDto>;
