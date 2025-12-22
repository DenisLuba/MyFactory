using MediatR;

namespace MyFactory.Application.Features.Suppliers.CreateSupplier;

public sealed record CreateSupplierCommand(
    string Name,
    string? Description
) : IRequest<Guid>;
