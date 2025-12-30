using FluentValidation;

namespace MyFactory.Application.Features.Positions.GetPositions;

public sealed class GetPositionsQueryValidator
    : AbstractValidator<GetPositionsQuery>
{
    public GetPositionsQueryValidator()
    {
        RuleFor(x => x.SortBy)
            .IsInEnum();
    }
}
