using MediatR;
using MyFactory.Application.DTOs.ProductTypes;

namespace MyFactory.Application.Features.ProductTypes.GetProductTypeByProductId;

public sealed record GetProductTypeByProductIdQuery(Guid ProductId) : IRequest<ProductTypeDto?>;
