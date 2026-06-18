using FluentValidation;

namespace MyFactory.Application.Features.Warehouses.AddProductToWarehouse;

public sealed class AddProductToWarehouseCommandValidator
    : AbstractValidator<AddProductToWarehouseCommand>
{
    public AddProductToWarehouseCommandValidator()
    {
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.QtyPerPackage).GreaterThan(0);
        RuleFor(x => x.PackageCount)
            .GreaterThan(0).When(x => x.PackageCount.HasValue);
    }
}
