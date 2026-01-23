using FluentValidation;

namespace MyFactory.Application.Features.Warehouses.AddProductToWarehouse;

public sealed class AddProductToWarehouseCommandValidator
    : AbstractValidator<AddProductToWarehouseCommand>
{
    public AddProductToWarehouseCommandValidator()
    {
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Qty).GreaterThan(0);
    }
}
