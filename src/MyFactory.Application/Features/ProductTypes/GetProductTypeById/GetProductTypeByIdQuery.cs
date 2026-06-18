using MediatR;

using MyFactory.Application.DTOs.ProductTypes;

namespace MyFactory.Application.Features.ProductTypes.GetProductTypeById;

public sealed record GetProductTypeByIdQuery(Guid ProductTypeId) : IRequest<ProductTypeDto?>;
