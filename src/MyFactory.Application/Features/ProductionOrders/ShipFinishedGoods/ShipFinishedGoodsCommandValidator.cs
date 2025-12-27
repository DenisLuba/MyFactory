using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.ShipFinishedGoods;

public sealed class ShipFinishedGoodsCommandValidator : AbstractValidator<ShipFinishedGoodsCommand>
{
    public ShipFinishedGoodsCommandValidator()
    {
        RuleFor(x => x.ProductionOrderId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.Qty).GreaterThan(0);
    }
}
