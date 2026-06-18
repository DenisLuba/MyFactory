using MediatR;

namespace MyFactory.Application.Features.Units.AddUnit;

public sealed record AddUnitCommand(string Code, string Name) : IRequest<Guid>;
