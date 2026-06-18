using FluentValidation;

namespace MyFactory.Application.Features.ProductTypes.UpdateProductType;

public sealed class UpdateProductTypeCommandValidator : AbstractValidator<UpdateProductTypeCommand>
{
    public UpdateProductTypeCommandValidator()
    {
        RuleFor(x => x.ProductTypeId)
            .NotEmpty();

        RuleFor(x => x.Type)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(1000);
    }
}
