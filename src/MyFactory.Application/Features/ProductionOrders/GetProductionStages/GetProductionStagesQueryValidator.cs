using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionStages;

public sealed class GetProductionStagesQueryValidator : AbstractValidator<GetProductionStagesQuery>
{
    public GetProductionStagesQueryValidator()
    {
        RuleFor(x => x.ProductionOrderId).NotEmpty();
    }
}
