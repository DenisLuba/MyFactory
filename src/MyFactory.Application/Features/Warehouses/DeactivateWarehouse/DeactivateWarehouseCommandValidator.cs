using FluentValidation;

namespace MyFactory.Application.Features.Warehouses.DeactivateWarehouse;

public sealed class DeactivateWarehouseCommandValidator
    : AbstractValidator<DeactivateWarehouseCommand>
{
    public DeactivateWarehouseCommandValidator()
    {
        RuleFor(x => x.WarehouseId)
            .NotEmpty();
    }
}