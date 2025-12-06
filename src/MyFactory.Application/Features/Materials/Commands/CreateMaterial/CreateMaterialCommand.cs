using System;
using MediatR;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.Features.Materials.Commands.CreateMaterial;

public sealed record CreateMaterialCommand(string Name, Guid MaterialTypeId, string Unit) : IRequest<MaterialDto>;
