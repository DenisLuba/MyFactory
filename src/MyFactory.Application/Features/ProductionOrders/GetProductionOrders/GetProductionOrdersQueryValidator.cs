using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrders;

public sealed class GetProductionOrdersQueryValidator : AbstractValidator<GetProductionOrdersQuery>
{
    public GetProductionOrdersQueryValidator()
    {
    }
}
