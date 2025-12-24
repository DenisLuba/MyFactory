using MediatR;

namespace MyFactory.Application.Features.Products.UpdateProductMaterial;

public sealed record UpdateProductMaterialCommand(
    Guid ProductMaterialId,
    decimal QtyPerUnit
) : IRequest;