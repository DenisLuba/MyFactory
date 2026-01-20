using MediatR;

namespace MyFactory.Application.Features.Products.RemoveProductMaterial;

public sealed record RemoveProductMaterialCommand(
    Guid ProductId,
    Guid MaterialId
) : IRequest;