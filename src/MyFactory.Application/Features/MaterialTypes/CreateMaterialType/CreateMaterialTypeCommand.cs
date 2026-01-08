using MediatR;

namespace MyFactory.Application.Features.MaterialTypes.CreateMaterialType;

public sealed record CreateMaterialTypeCommand(string Name, string? Description) : IRequest<Guid>;
