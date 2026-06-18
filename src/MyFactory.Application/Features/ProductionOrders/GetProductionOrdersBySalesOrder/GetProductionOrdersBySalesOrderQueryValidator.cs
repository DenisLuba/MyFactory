using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrdersBySalesOrder;

public sealed class GetProductionOrdersBySalesOrderQueryValidator : AbstractValidator<GetProductionOrdersBySalesOrderQuery>
{
    public GetProductionOrdersBySalesOrderQueryValidator()
    {
        RuleFor(x => x.SalesOrderId).NotEmpty();
    }
}
