using FluentValidation;

namespace MyFactory.Application.Features.SalesOrders.UpdateSalesOrder;

public sealed class UpdateSalesOrderCommandValidator : AbstractValidator<UpdateSalesOrderCommand>
{
    public UpdateSalesOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.OrderDate).NotEmpty().Must(date => date != default);
    }
}
