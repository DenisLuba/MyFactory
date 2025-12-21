using System;
using MediatR;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.OldFeatures.Materials.Queries.GetMaterialById;

public sealed record GetMaterialByIdQuery(Guid Id) : IRequest<MaterialDto>;
