using MediatR;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.Features.Materials.GetMaterialImage;

public sealed record GetMaterialImageQuery(Guid ImageId) : IRequest<MaterialImageDto?>;
