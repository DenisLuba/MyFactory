using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.StartNextProductionStage;

public sealed class StartNextProductionStageCommandValidator
    : AbstractValidator<StartNextProductionStageCommand>
{
    public StartNextProductionStageCommandValidator()
    {
        RuleFor(x => x.ProductionOrderId).NotEmpty();
    }
}
