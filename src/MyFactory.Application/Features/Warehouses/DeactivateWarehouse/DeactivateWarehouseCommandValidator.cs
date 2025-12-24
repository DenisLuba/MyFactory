using FluentValidation;

namespace MyFactory.Application.Features.Warehouses.GetWarehouses;

public sealed class DeactivateWarehouseCommandValidator
    : AbstractValidator<DeactivateWarehouseCommand>
{
    public DeactivateWarehouseCommandValidator()
    {
        RuleFor(x => x.WarehouseId)
            .NotEmpty();
    }
}