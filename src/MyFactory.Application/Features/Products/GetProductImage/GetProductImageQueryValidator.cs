using FluentValidation;

namespace MyFactory.Application.Features.Products.GetProductImage;

public sealed class GetProductImageQueryValidator : AbstractValidator<GetProductImageQuery>
{
    public GetProductImageQueryValidator()
    {
        RuleFor(x => x.ImageId).NotEmpty();
    }
}
