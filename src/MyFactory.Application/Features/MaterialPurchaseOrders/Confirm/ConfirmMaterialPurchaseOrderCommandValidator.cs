using FluentValidation;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.Confirm;

public sealed class ConfirmMaterialPurchaseOrderCommandValidator
    : AbstractValidator<ConfirmMaterialPurchaseOrderCommand>
{
    public ConfirmMaterialPurchaseOrderCommandValidator()
    {
        RuleFor(x => x.PurchaseOrderId).NotEmpty();
    }
}
