using FluentValidation;

namespace MyFactory.Application.Features.Warehouses.ActivateWarehouse;

public class ActivateWarehouseCommandValidator : AbstractValidator<ActivateWarehouseCommand>
{
    public ActivateWarehouseCommandValidator()
    {
        RuleFor(x => x.WarehouseId)
            .NotEmpty()
            .WithMessage("WarehouseId cannot be empty.");
    }
}
