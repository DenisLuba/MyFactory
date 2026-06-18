using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.CreateProductionOrder;

public sealed class CreateProductionOrderCommandValidator : AbstractValidator<CreateProductionOrderCommand>
{
    public CreateProductionOrderCommandValidator()
    {
        RuleFor(x => x.SalesOrderItemId).NotEmpty();
        RuleFor(x => x.DepartmentId).NotEmpty();
        RuleFor(x => x.QtyPlanned).GreaterThan(0);
    }
}
