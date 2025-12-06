using System;
using MediatR;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.Features.Materials.Queries.GetMaterialById;

public sealed record GetMaterialByIdQuery(Guid Id) : IRequest<MaterialDto>;
