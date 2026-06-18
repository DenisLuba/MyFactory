using FluentValidation;

namespace MyFactory.Application.Features.Products.RemoveProductMaterial;

public sealed class RemoveProductMaterialCommandValidator
    : AbstractValidator<RemoveProductMaterialCommand>
{
    public RemoveProductMaterialCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();
        RuleFor(x => x.MaterialId)
            .NotEmpty();
    }
}