using FluentValidation;
using MyFactory.Application.Features.Warehouses.UpdateWarehouseMaterialQty;

namespace MyFactory.Application.Features.Warehouses.RemoveMaterialFromWarehouse;

public sealed class RemoveMaterialFromWarehouseCommandValidator
    : AbstractValidator<RemoveMaterialFromWarehouseCommand>
{
    public RemoveMaterialFromWarehouseCommandValidator()
    {
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}