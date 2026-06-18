using FluentValidation;

namespace MyFactory.Application.Features.Warehouses.GetWarehouses;

public sealed class CreateWarehouseCommandValidator
    : AbstractValidator<CreateWarehouseCommand>
{
    public CreateWarehouseCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Type)
            .IsInEnum();
    }
}