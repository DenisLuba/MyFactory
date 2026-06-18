using FluentValidation;

namespace MyFactory.Application.Features.SalesOrders.CancelSalesOrder;

public sealed class CancelSalesOrderCommandValidator : AbstractValidator<CancelSalesOrderCommand>
{
    public CancelSalesOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
    }
}
