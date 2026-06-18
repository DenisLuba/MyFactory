using FluentValidation;

namespace MyFactory.Application.Features.Products.UploadProductImage;

public sealed class UploadProductImageCommandValidator : AbstractValidator<UploadProductImageCommand>
{
    public UploadProductImageCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.FileName).NotEmpty();
        RuleFor(x => x.Content).NotNull().Must(c => c.Length > 0).WithMessage("File content is required.");
    }
}
