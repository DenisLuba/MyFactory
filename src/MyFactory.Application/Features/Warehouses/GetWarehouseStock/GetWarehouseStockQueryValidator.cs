using FluentValidation;

namespace MyFactory.Application.Features.Warehouses.GetWarehouseStock;

public sealed class GetWarehouseStockQueryValidator
    : AbstractValidator<GetWarehouseStockQuery>
{
    public GetWarehouseStockQueryValidator()
    {
        RuleFor(x => x.WarehouseId)
            .NotEmpty();
    }
}