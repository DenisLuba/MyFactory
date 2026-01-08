using MediatR;
using MyFactory.Application.DTOs.MaterialTypes;

namespace MyFactory.Application.Features.MaterialTypes.GetMaterialTypes;

public sealed record GetMaterialTypesQuery : IRequest<IReadOnlyList<MaterialTypeDto>>;
