using FluentValidation;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.Receive;

public sealed class ReceiveMaterialPurchaseOrderCommandValidator
    : AbstractValidator<ReceiveMaterialPurchaseOrderCommand>
{
    public ReceiveMaterialPurchaseOrderCommandValidator()
    {
        RuleFor(x => x.PurchaseOrderId).NotEmpty();
        RuleFor(x => x.Allocations)
            .NotNull()
            .NotEmpty();

        RuleForEach(x => x.Allocations).ChildRules(item =>
        {
            item.RuleFor(i => i.WarehouseId).NotEmpty();
            item.RuleFor(i => i.MateialItems)
                .NotNull()
                .NotEmpty();
            item.RuleForEach(i => i.MateialItems).ChildRules(a =>
            {
                a.RuleFor(p => p.MaterialId).NotEmpty();
                a.RuleFor(p => p.QtyPerPackage).GreaterThan(0);
                a.RuleFor(p => p.PackageCount)
                    .GreaterThan(0).When(p => p.PackageCount.HasValue);
            });
        });
    }
}
