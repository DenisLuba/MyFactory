using MediatR;
using MyFactory.Domain.Entities.Products;

namespace MyFactory.Application.Features.Products.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid ProductId,
    string Name,
    int? PlanPerHour,
    ProductStatus Status,
    string? Description,
    int? Version
) : IRequest<Guid>;