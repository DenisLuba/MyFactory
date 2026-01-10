using FluentValidation;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.Receive;

public sealed class ReceiveMaterialPurchaseOrderCommandValidator
    : AbstractValidator<ReceiveMaterialPurchaseOrderCommand>
{
    public ReceiveMaterialPurchaseOrderCommandValidator()
    {
        RuleFor(x => x.PurchaseOrderId).NotEmpty();
        RuleFor(x => x.Items)
            .NotNull()
            .NotEmpty();

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ItemId).NotEmpty();
            item.RuleFor(i => i.Allocations)
                .NotNull()
                .NotEmpty();
            item.RuleForEach(i => i.Allocations).ChildRules(a =>
            {
                a.RuleFor(p => p.WarehouseId).NotEmpty();
                a.RuleFor(p => p.Qty).GreaterThan(0);
            });
        });
    }
}
