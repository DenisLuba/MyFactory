using FluentValidation;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.RemoveItem;

public sealed class RemoveMaterialPurchaseOrderItemCommandValidator
    : AbstractValidator<RemoveMaterialPurchaseOrderItemCommand>
{
    public RemoveMaterialPurchaseOrderItemCommandValidator()
    {
        RuleFor(x => x.ItemId).NotEmpty();
    }
}
