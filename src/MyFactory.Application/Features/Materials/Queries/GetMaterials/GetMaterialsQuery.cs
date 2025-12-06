using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.Features.Materials.Queries.GetMaterials;

public sealed record GetMaterialsQuery : IRequest<IReadOnlyCollection<MaterialDto>>;
