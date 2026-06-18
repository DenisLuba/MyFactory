using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.IssueMaterialsToProduction;

public sealed class IssueMaterialsToProductionCommandValidator : AbstractValidator<IssueMaterialsToProductionCommand>
{
    public IssueMaterialsToProductionCommandValidator()
    {
        RuleFor(x => x.ProductionOrderId).NotEmpty();
    }
}
