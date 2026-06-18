using FluentValidation;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.AddItem;

public sealed class AddMaterialPurchaseOrderItemCommandValidator
    : AbstractValidator<AddMaterialPurchaseOrderItemCommand>
{
    public AddMaterialPurchaseOrderItemCommandValidator()
    {
        RuleFor(x => x.PurchaseOrderId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
        RuleFor(x => x.Qty).GreaterThan(0);
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
    }
}
