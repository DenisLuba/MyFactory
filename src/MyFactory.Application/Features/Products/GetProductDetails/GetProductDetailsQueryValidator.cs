using FluentValidation;

namespace MyFactory.Application.Features.Products.GetProductDetails;

public sealed class GetProductDetailsQueryValidator
    : AbstractValidator<GetProductDetailsQuery>
{
    public GetProductDetailsQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();
    }
}