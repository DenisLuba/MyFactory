using MediatR;
using MyFactory.Application.DTOs.Products;

namespace MyFactory.Application.Features.Products.GetProductDetails;

public sealed record GetProductDetailsQuery(Guid ProductId)
    : IRequest<ProductDetailsDto>;

