using FluentValidation;
using MyFactory.Application.Features.FinishedGoods.Queries.GetReturnById;

namespace MyFactory.Application.OldFeatures.FinishedGoods.Validators;

public sealed class GetReturnByIdQueryValidator : AbstractValidator<GetReturnByIdQuery>
{
    public GetReturnByIdQueryValidator()
    {
        RuleFor(query => query.ReturnId).NotEmpty();
    }
}
