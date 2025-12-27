using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.CancelProductionOrder;

public sealed class CancelProductionOrderCommandValidator : AbstractValidator<CancelProductionOrderCommand>
{
    public CancelProductionOrderCommandValidator()
    {
        RuleFor(x => x.ProductionOrderId).NotEmpty();
    }
}
