using FluentValidation;

namespace MyFactory.Application.Features.SalesOrders.GetSalesOrders;

public sealed class GetSalesOrdersQueryValidator 
    : AbstractValidator<GetSalesOrdersQuery>
{
    public GetSalesOrdersQueryValidator()
    {
        // Нет правил — валидатор пустой намеренно
    }
}
