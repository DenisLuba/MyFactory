using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.CompleteProductionStage;

public sealed class CompleteProductionStageCommandValidator : AbstractValidator<CompleteProductionStageCommand>
{
    public CompleteProductionStageCommandValidator()
    {
        RuleFor(x => x.ProductionOrderId).NotEmpty();
        RuleFor(x => x.Qty).GreaterThan(0);
    }
}
