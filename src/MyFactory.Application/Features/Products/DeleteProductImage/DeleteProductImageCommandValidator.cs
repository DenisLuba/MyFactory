using FluentValidation;

namespace MyFactory.Application.Features.Products.DeleteProductImage;

public sealed class DeleteProductImageCommandValidator : AbstractValidator<DeleteProductImageCommand>
{
    public DeleteProductImageCommandValidator()
    {
        RuleFor(x => x.ImageId).NotEmpty();
    }
}
