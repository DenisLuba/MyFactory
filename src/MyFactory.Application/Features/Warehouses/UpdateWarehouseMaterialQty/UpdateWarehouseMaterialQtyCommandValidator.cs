using FluentValidation;

namespace MyFactory.Application.Features.Warehouses.UpdateWarehouseMaterialQty;

public sealed class UpdateWarehouseMaterialQtyCommandValidator
    : AbstractValidator<UpdateWarehouseMaterialQtyCommand>
{
    public UpdateWarehouseMaterialQtyCommandValidator()
    {
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
        RuleFor(x => x.Qty).GreaterThanOrEqualTo(0);
    }
}