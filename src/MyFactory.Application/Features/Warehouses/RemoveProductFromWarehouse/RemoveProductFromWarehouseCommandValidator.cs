using FluentValidation;

namespace MyFactory.Application.Features.Warehouses.RemoveProductFromWarehouse;

public sealed class RemoveProductFromWarehouseCommandValidator
    : AbstractValidator<RemoveProductFromWarehouseCommand>
{
    public RemoveProductFromWarehouseCommandValidator()
    {
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
    }
}