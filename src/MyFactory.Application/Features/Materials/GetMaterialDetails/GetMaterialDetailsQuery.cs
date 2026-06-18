using MediatR;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.Features.Materials.GetMaterialDetails;

public sealed record GetMaterialDetailsQuery(Guid MaterialId) : IRequest<MaterialDetailsDto>;
