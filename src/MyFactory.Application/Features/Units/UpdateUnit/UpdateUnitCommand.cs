using MediatR;

namespace MyFactory.Application.Features.Units.UpdateUnit;

public sealed record UpdateUnitCommand(Guid UnitId, string Code, string Name) : IRequest;
