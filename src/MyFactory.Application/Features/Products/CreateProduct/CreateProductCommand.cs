using MediatR;
using MyFactory.Domain.Entities.Products;

namespace MyFactory.Application.Features.Products.CreateProduct;

public sealed record CreateProductCommand(
    string Sku,
    string Name,
    ProductStatus Status,
    int? PlanPerHour,
    string? Description,
    int? Version
) : IRequest<Guid>;

