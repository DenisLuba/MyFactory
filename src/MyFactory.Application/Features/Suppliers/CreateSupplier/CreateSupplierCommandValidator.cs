using FluentValidation;

namespace MyFactory.Application.Features.Suppliers.CreateSupplier;

public sealed class CreateSupplierCommandValidator
    : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);
    }
}
