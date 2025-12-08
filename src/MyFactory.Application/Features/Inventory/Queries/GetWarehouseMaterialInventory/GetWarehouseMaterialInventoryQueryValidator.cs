using FluentValidation;

namespace MyFactory.Application.Features.Inventory.Queries.GetWarehouseMaterialInventory;

public sealed class GetWarehouseMaterialInventoryQueryValidator : AbstractValidator<GetWarehouseMaterialInventoryQuery>
{
    public GetWarehouseMaterialInventoryQueryValidator()
    {
        RuleFor(query => query.MaterialId).NotEmpty();
        RuleFor(query => query.WarehouseId).NotEmpty();
    }
}
