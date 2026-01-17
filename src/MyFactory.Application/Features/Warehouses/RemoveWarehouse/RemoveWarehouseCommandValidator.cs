using FluentValidation;

namespace MyFactory.Application.Features.Warehouses.RemoveWarehouse;

public class RemoveWarehouseCommandValidator : AbstractValidator<RemoveWarehouseCommand>
{
    public RemoveWarehouseCommandValidator()
    {
        RuleFor(x => x.WarehouseId)
            .NotEmpty()
            .WithMessage("WarehouseId must not be empty.");
    }
}
