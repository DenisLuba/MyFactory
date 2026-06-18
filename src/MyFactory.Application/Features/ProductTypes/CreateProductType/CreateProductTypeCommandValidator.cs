using FluentValidation;

namespace MyFactory.Application.Features.ProductTypes.CreateProductType;

public sealed class CreateProductTypeCommandValidator : AbstractValidator<CreateProductTypeCommand>
{
    public CreateProductTypeCommandValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(1000);
    }
}
