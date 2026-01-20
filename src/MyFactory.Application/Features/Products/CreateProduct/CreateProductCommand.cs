using MediatR;
using MyFactory.Domain.Entities.Products;

namespace MyFactory.Application.Features.Products.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    ProductStatus Status,
    decimal? PlanPerHour,
    string? Description,
    decimal? Version
) : IRequest<Guid>;

