using MediatR;

namespace MyFactory.Application.Features.Units.RemoveUnit;

public sealed record RemoveUnitCommand(Guid UnitId) : IRequest;
