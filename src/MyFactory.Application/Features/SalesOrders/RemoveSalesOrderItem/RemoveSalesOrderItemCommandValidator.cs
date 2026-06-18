using FluentValidation;

namespace MyFactory.Application.Features.SalesOrders.RemoveSalesOrderItem;

public sealed class RemoveSalesOrderItemCommandValidator : AbstractValidator<RemoveSalesOrderItemCommand>
{
    public RemoveSalesOrderItemCommandValidator()
    {
        RuleFor(x => x.OrderItemId).NotEmpty();
    }
}
