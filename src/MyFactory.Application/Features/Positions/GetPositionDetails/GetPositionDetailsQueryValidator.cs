using FluentValidation;

namespace MyFactory.Application.Features.Positions.GetPositionDetails;

public sealed class GetPositionDetailsQueryValidator
    : AbstractValidator<GetPositionDetailsQuery>
{
    public GetPositionDetailsQueryValidator()
    {
        RuleFor(x => x.PositionId)
            .NotEmpty();
    }
}
