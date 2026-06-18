using FluentValidation;

namespace MyFactory.Application.Features.Products.AddProductMaterial;

public sealed class AddProductMaterialCommandValidator
    : AbstractValidator<AddProductMaterialCommand>
{
    public AddProductMaterialCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.MaterialId)
            .NotEmpty();

        RuleFor(x => x.QtyPerUnit)
            .GreaterThan(0);
    }
}