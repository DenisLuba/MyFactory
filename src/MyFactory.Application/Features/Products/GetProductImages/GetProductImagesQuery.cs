using MediatR;
using MyFactory.Application.DTOs.Products;

namespace MyFactory.Application.Features.Products.GetProductImages;

public sealed record GetProductImagesQuery(Guid ProductId) : IRequest<IReadOnlyList<ProductImageDto>>;
