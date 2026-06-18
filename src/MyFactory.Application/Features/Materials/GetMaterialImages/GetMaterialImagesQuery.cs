using MediatR;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.Features.Materials.GetMaterialImages;

public sealed record GetMaterialImagesQuery(Guid MaterialId) : IRequest<IReadOnlyList<MaterialImageDto>>;
