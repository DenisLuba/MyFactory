using FluentValidation;

namespace MyFactory.Application.Features.ProductTypes.DeleteProductType;

public sealed class DeleteProductTypeCommandValidator : AbstractValidator<DeleteProductTypeCommand>
{
    public DeleteProductTypeCommandValidator()
    {
        RuleFor(x => x.ProductTypeId)
            .NotEmpty();
    }
}
