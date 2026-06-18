using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.StartProductionStage;

public sealed class StartProductionStageCommandValidator : AbstractValidator<StartProductionStageCommand>
{
    public StartProductionStageCommandValidator()
    {
        RuleFor(x => x.ProductionOrderId).NotEmpty();
        RuleFor(x => x.TargetStatus).IsInEnum();
    }
}
