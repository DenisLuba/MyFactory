using MediatR;

namespace MyFactory.Application.Features.Suppliers.UpdateSupplier;

public sealed record UpdateSupplierCommand : IRequest
{
    public Guid SupplierId { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
}
