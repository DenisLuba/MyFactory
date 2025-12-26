using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrderDetails;

public sealed class GetProductionOrderDetailsQueryValidator : AbstractValidator<GetProductionOrderDetailsQuery>
{
    public GetProductionOrderDetailsQueryValidator()
    {
        RuleFor(x => x.ProductionOrderId).NotEmpty();
    }
}
