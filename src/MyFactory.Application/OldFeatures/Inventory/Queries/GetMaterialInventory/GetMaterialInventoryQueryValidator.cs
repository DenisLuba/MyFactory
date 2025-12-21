using FluentValidation;

namespace MyFactory.Application.OldFeatures.Inventory.Queries.GetMaterialInventory;

public sealed class GetMaterialInventoryQueryValidator : AbstractValidator<GetMaterialInventoryQuery>
{
    public GetMaterialInventoryQueryValidator()
    {
        RuleFor(query => query.MaterialId).NotEmpty();
    }
}
