using MediatR;

namespace MyFactory.Application.Features.Products.RemoveProductMaterial;

public sealed record RemoveProductMaterialCommand(
    Guid ProductMaterialId
) : IRequest;