using MediatR;
using MyFactory.Application.DTOs.Positions;

namespace MyFactory.Application.Features.Positions.GetPositions;

public sealed record GetPositionsQuery(
    Guid? DepartmentId,
    bool IncludeInactive = false,
    PositionSortBy SortBy = PositionSortBy.Name,
    bool SortDesc = false
) : IRequest<IReadOnlyList<PositionListItemDto>>;

public enum PositionSortBy
{
    Name,
    Code
}