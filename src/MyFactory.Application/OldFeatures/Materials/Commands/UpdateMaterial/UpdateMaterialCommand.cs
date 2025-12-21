using System;
using MediatR;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.OldFeatures.Materials.Commands.UpdateMaterial;

public sealed record UpdateMaterialCommand(
    Guid Id,
    string? Name,
    Guid? MaterialTypeId,
    string? Unit,
    bool? IsActive) : IRequest<MaterialDto>;
