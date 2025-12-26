using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.DeleteProductionOrder;

public sealed class DeleteProductionOrderCommandValidator : AbstractValidator<DeleteProductionOrderCommand>
{
    public DeleteProductionOrderCommandValidator()
    {
        RuleFor(x => x.ProductionOrderId).NotEmpty();
    }
}
