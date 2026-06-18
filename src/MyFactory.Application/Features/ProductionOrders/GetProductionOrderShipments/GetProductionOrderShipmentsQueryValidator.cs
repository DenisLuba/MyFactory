using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrderShipments;

public sealed class GetProductionOrderShipmentsQueryValidator : AbstractValidator<GetProductionOrderShipmentsQuery>
{
    public GetProductionOrderShipmentsQueryValidator()
    {
        RuleFor(x => x.ProductionOrderId).NotEmpty();
    }
}
