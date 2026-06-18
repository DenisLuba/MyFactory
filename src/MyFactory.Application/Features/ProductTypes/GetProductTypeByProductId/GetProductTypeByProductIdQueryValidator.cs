using FluentValidation;

namespace MyFactory.Application.Features.ProductTypes.GetProductTypeByProductId;

public sealed class GetProductTypeByProductIdQueryValidator : AbstractValidator<GetProductTypeByProductIdQuery>
{
    public GetProductTypeByProductIdQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();
    }
}
