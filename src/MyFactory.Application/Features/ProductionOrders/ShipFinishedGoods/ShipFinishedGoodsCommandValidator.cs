using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.ShipFinishedGoods;

public sealed class ShipFinishedGoodsCommandValidator : AbstractValidator<ShipFinishedGoodsCommand>
{
    public ShipFinishedGoodsCommandValidator()
    {
        RuleFor(x => x.ProductionOrderId).NotEmpty();
        RuleFor(x => x.FromWarehouseId).NotEmpty();
        RuleFor(x => x.ToWarehouseId).NotEmpty();
        RuleFor(x => x.QtyPerPackage).GreaterThan(0);
        RuleFor(x => x.PackageCount)
            .GreaterThan(0).When(x => x.PackageCount.HasValue);

    }
}
