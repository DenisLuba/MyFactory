using FluentValidation;

namespace MyFactory.Application.Features.Suppliers.DeleteSupplier;

public sealed class DeleteSupplierCommandValidator
    : AbstractValidator<DeleteSupplierCommand>
{
    public DeleteSupplierCommandValidator()
    {
        RuleFor(x => x.SupplierId)
            .NotEmpty();
    }
}
