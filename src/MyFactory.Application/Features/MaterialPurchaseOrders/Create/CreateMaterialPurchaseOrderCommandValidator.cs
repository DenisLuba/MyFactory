using FluentValidation;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.Create;

public sealed class CreateMaterialPurchaseOrderCommandValidator
    : AbstractValidator<CreateMaterialPurchaseOrderCommand>
{
    public CreateMaterialPurchaseOrderCommandValidator()
    {
        RuleFor(x => x.SupplierId).NotEmpty();
        RuleFor(x => x.OrderDate).NotEmpty();
    }
}