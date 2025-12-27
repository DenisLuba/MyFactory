using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrderMaterialIssueDetails;

public sealed class GetProductionOrderMaterialIssueDetailsQueryValidator
    : AbstractValidator<GetProductionOrderMaterialIssueDetailsQuery>
{
    public GetProductionOrderMaterialIssueDetailsQueryValidator()
    {
        RuleFor(x => x.ProductionOrderId)
            .NotEmpty();

        RuleFor(x => x.MaterialId)
            .NotEmpty();
    }
}