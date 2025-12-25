using FluentValidation;

namespace MyFactory.Application.Features.SalesOrders.GetSalesOrderDetails;

public sealed class GetSalesOrderDetailsQueryValidator : AbstractValidator<GetSalesOrderDetailsQuery>
{
    public GetSalesOrderDetailsQueryValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
    }
}
