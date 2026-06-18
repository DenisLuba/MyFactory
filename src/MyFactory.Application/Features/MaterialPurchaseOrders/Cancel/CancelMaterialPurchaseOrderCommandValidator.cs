using FluentValidation;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.Cancel;

public sealed class CancelMaterialPurchaseOrderCommandValidator
    : AbstractValidator<CancelMaterialPurchaseOrderCommand>
{
    public CancelMaterialPurchaseOrderCommandValidator()
    {
        RuleFor(x => x.PurchaseOrderId).NotEmpty();
    }
}
