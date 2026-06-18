using MediatR;

namespace MyFactory.Application.Features.MaterialTypes.UpdateMaterialType;

public sealed record UpdateMaterialTypeCommand(Guid Id, string Name, string? Description) : IRequest;
