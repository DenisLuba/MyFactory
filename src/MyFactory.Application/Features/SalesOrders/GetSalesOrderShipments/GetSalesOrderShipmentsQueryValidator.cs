using FluentValidation;

namespace MyFactory.Application.Features.SalesOrders.GetSalesOrderShipments;

public sealed class GetSalesOrderShipmentsQueryValidator : AbstractValidator<GetSalesOrderShipmentsQuery>
{
    public GetSalesOrderShipmentsQueryValidator()
    {
        RuleFor(x => x.SalesOrderId).NotEmpty();
    }
}
