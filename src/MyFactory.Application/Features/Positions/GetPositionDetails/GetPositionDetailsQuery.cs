using MediatR;
using MyFactory.Application.DTOs.Positions;

namespace MyFactory.Application.Features.Positions.GetPositionDetails;

public sealed record GetPositionDetailsQuery(
    Guid PositionId
) : IRequest<PositionDetailsDto>;
