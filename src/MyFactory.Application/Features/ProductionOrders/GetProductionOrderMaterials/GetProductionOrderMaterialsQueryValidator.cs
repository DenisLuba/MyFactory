using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrderMaterials;

public sealed class GetProductionOrderMaterialsQueryValidator : AbstractValidator<GetProductionOrderMaterialsQuery>
{
    public GetProductionOrderMaterialsQueryValidator()
    {
        RuleFor(x => x.ProductionOrderId).NotEmpty();
    }
}
