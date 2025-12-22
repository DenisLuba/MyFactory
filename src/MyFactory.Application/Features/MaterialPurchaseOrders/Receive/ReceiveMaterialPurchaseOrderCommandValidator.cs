using FluentValidation;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.Receive;

public sealed class ReceiveMaterialPurchaseOrderCommandValidator
    : AbstractValidator<ReceiveMaterialPurchaseOrderCommand>
{
    public ReceiveMaterialPurchaseOrderCommandValidator()
    {
        RuleFor(x => x.PurchaseOrderId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ReceiveDate).NotEmpty();
    }
}
