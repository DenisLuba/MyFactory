using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.UpdateProductionOrder;

public sealed class UpdateProductionOrderCommandValidator : AbstractValidator<UpdateProductionOrderCommand>
{
    public UpdateProductionOrderCommandValidator()
    {
        RuleFor(x => x.ProductionOrderId).NotEmpty();
        RuleFor(x => x.DepartmentId).NotEmpty();
        RuleFor(x => x.QtyPlanned).GreaterThan(0);
    }
}
