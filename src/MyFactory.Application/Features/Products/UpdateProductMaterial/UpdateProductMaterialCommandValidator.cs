using FluentValidation;

namespace MyFactory.Application.Features.Products.UpdateProductMaterial;

public sealed class UpdateProductMaterialCommandValidator
    : AbstractValidator<UpdateProductMaterialCommand>
{
    public UpdateProductMaterialCommandValidator()
    {
        RuleFor(x => x.ProductMaterialId)
            .NotEmpty();

        RuleFor(x => x.QtyPerUnit)
            .GreaterThan(0);
    }
}