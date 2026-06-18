using FluentValidation;

namespace MyFactory.Application.Features.Warehouses.UpdateWarehouseProductQty;

public sealed class UpdateWarehouseProductQtyCommandValidator
    : AbstractValidator<UpdateWarehouseProductQtyCommand>
{
    public UpdateWarehouseProductQtyCommandValidator()
    {
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.QtyPerPackage).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PackageCount)
            .GreaterThanOrEqualTo(0).When(x => x.PackageCount.HasValue);
    }
}