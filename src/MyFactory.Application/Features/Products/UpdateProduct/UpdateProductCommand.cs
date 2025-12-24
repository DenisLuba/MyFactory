using MediatR;
using MyFactory.Domain.Entities.Products;

namespace MyFactory.Application.Features.Products.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid ProductId,
    string Name,
    decimal? PlanPerHour,
    ProductStatus Status
) : IRequest<Guid>;