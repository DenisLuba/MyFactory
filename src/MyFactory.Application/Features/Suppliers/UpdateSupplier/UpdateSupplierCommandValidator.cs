using FluentValidation;

namespace MyFactory.Application.Features.Suppliers.UpdateSupplier;

public sealed class UpdateSupplierCommandValidator
    : AbstractValidator<UpdateSupplierCommand>
{
    public UpdateSupplierCommandValidator()
    {
        RuleFor(x => x.SupplierId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
