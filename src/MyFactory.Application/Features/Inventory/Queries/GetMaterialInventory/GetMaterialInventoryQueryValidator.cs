using FluentValidation;

namespace MyFactory.Application.Features.Inventory.Queries.GetMaterialInventory;

public sealed class GetMaterialInventoryQueryValidator : AbstractValidator<GetMaterialInventoryQuery>
{
    public GetMaterialInventoryQueryValidator()
    {
        RuleFor(query => query.MaterialId).NotEmpty();
    }
}
