using FluentValidation;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.UpdateItem;

public sealed class UpdateMaterialPurchaseOrderItemCommandValidator
    : AbstractValidator<UpdateMaterialPurchaseOrderItemCommand>
{
    public UpdateMaterialPurchaseOrderItemCommandValidator()
    {
        RuleFor(x => x.ItemId).NotEmpty();
        RuleFor(x => x.Qty).GreaterThan(0);
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
    }
}
