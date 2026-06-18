using FluentValidation;

namespace MyFactory.Application.Features.Warehouses.GetWarehouses;

public sealed class UpdateWarehouseCommandValidator
    : AbstractValidator<UpdateWarehouseCommand>
{
    public UpdateWarehouseCommandValidator()
    {
        RuleFor(x => x.WarehouseId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Type)
            .IsInEnum();
    }
}