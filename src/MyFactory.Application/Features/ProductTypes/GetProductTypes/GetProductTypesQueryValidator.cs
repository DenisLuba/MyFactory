using FluentValidation;

namespace MyFactory.Application.Features.ProductTypes.GetProductTypes;

public sealed class GetProductTypesQueryValidator : AbstractValidator<GetProductTypesQuery>
{
    public GetProductTypesQueryValidator()
    {
        RuleFor(x => x.Search)
            .MaximumLength(100);
    }
}
