using MediatR;
using MyFactory.Application.DTOs.ProductTypes;

namespace MyFactory.Application.Features.ProductTypes.GetProductTypes;

public sealed record GetProductTypesQuery(string? Search = null) : IRequest<IReadOnlyList<ProductTypeDto>>;
