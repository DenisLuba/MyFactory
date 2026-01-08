using MediatR;

namespace MyFactory.Application.Features.Materials.RemoveMaterial;

public sealed record RemoveMaterialCommand(Guid MaterialId) : IRequest;
