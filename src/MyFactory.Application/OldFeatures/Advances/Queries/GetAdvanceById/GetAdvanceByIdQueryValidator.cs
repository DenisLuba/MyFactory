using FluentValidation;

namespace MyFactory.Application.OldFeatures.Advances.Queries.GetAdvanceById;

public sealed class GetAdvanceByIdQueryValidator : AbstractValidator<GetAdvanceByIdQuery>
{
    public GetAdvanceByIdQueryValidator()
    {
        RuleFor(query => query.AdvanceId).NotEmpty();
    }
}
