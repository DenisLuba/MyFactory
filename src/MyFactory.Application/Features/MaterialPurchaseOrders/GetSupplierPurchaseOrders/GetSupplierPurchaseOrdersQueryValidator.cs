using FluentValidation;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.GetSupplierPurchaseOrders;

public sealed class GetSupplierPurchaseOrdersQueryValidator
    : AbstractValidator<GetSupplierPurchaseOrdersQuery>
{
    public GetSupplierPurchaseOrdersQueryValidator()
    {
        RuleFor(x => x.SupplierId).NotEmpty();
    }
}
