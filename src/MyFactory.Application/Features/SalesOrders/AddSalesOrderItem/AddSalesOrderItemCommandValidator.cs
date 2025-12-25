using FluentValidation;

namespace MyFactory.Application.Features.SalesOrders.AddSalesOrderItem;

public sealed class AddSalesOrderItemCommandValidator : AbstractValidator<AddSalesOrderItemCommand>
{
    public AddSalesOrderItemCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Qty).GreaterThan(0);
    }
}
