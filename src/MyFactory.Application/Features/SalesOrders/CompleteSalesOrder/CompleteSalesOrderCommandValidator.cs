using FluentValidation;

namespace MyFactory.Application.Features.SalesOrders.CompleteSalesOrder;

public sealed class CompleteSalesOrderCommandValidator : AbstractValidator<CompleteSalesOrderCommand>
{
    public CompleteSalesOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
    }
}
