using FluentValidation;

namespace MyFactory.Application.Features.SalesOrders.UpdateSalesOrderItem;

public sealed class UpdateSalesOrderItemCommandValidator : AbstractValidator<UpdateSalesOrderItemCommand>
{
    public UpdateSalesOrderItemCommandValidator()
    {
        RuleFor(x => x.OrderItemId).NotEmpty();
        RuleFor(x => x.Qty).GreaterThan(0);
    }
}
