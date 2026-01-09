using FluentValidation;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.GetDetails;

public sealed class GetMaterialPurchaseOrderDetailsQueryValidator
    : AbstractValidator<GetMaterialPurchaseOrderDetailsQuery>
{
    public GetMaterialPurchaseOrderDetailsQueryValidator()
    {
        RuleFor(x => x.PurchaseOrderId).NotEmpty();
    }
}
