using FluentValidation;

namespace MyFactory.Application.Features.SalesOrders.StartSalesOrder;

public sealed class StartSalesOrderCommandValidator : AbstractValidator<StartSalesOrderCommand>
{
    public StartSalesOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
    }
}
