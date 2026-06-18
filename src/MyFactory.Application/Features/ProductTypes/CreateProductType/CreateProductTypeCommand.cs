using MediatR;

namespace MyFactory.Application.Features.ProductTypes.CreateProductType;

public sealed record CreateProductTypeCommand(
    string Type,
    string? Description
) : IRequest<Guid>;
