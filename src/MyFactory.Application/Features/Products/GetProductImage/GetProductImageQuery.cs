using MediatR;
using MyFactory.Application.DTOs.Products;

namespace MyFactory.Application.Features.Products.GetProductImage;

public sealed record GetProductImageQuery(Guid ImageId) : IRequest<ProductImageDto?>;
