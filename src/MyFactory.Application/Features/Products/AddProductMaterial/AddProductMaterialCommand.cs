using MediatR;

namespace MyFactory.Application.Features.Products.AddProductMaterial;

public sealed record AddProductMaterialCommand : IRequest<Guid>
{
    public Guid ProductId { get; init; }
    public Guid MaterialId { get; init; }
    public decimal QtyPerUnit { get; init; }
}