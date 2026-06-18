using MediatR;

namespace MyFactory.Application.Features.ProductTypes.UpdateProductType;

public sealed record UpdateProductTypeCommand(
    Guid ProductTypeId,
    string Type,
    string? Description
) : IRequest<Guid>;
