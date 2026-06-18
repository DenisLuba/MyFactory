using MediatR;

namespace MyFactory.Application.Features.Materials.DeleteMaterialImage;

public sealed record DeleteMaterialImageCommand(Guid ImageId) : IRequest;
