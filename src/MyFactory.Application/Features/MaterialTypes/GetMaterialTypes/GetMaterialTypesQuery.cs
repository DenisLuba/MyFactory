using MediatR;
using MyFactory.Application.DTOs.MaterialTypes;

namespace MyFactory.Application.Features.MaterialTypes.GetMaterialTypes;

public sealed record GetMaterialTypesQuery(bool UsedOnly = false) : IRequest<IReadOnlyList<MaterialTypeDto>>;
