using FluentValidation;

namespace MyFactory.Application.Features.SalesOrders.DeleteSalesOrder;

public sealed class DeleteSalesOrderCommandValidator : AbstractValidator<DeleteSalesOrderCommand>
{
    public DeleteSalesOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
    }
}
