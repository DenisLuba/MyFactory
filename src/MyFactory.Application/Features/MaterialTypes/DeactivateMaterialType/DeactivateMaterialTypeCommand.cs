using MediatR;

namespace MyFactory.Application.Features.MaterialTypes.DeactivateMaterialType;

public sealed record DeactivateMaterialTypeCommand(Guid MaterialTypeId) : IRequest;
