using MediatR;

namespace MyFactory.Application.Features.Suppliers.DeleteSupplier;

public sealed record DeleteSupplierCommand(Guid SupplierId) : IRequest;
