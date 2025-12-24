using FluentValidation;

namespace MyFactory.Application.Features.Warehouses.AddMaterialToWarehouse;

public sealed class AddMaterialToWarehouseCommandValidator
    : AbstractValidator<AddMaterialToWarehouseCommand>
{
    public AddMaterialToWarehouseCommandValidator()
    {
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
        RuleFor(x => x.Qty).GreaterThan(0);
    }
}