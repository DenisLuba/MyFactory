using MediatR;
using MyFactory.Application.DTOs.Units;

namespace MyFactory.Application.Features.Units.GetUnits;

public sealed record GetUnitsQuery() : IRequest<IReadOnlyList<UnitDto>>;
