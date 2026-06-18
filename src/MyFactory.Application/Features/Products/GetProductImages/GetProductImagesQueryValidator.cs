using FluentValidation;

namespace MyFactory.Application.Features.Products.GetProductImages;

public sealed class GetProductImagesQueryValidator : AbstractValidator<GetProductImagesQuery>
{
    public GetProductImagesQueryValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
    }
}
