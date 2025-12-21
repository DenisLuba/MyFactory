using FluentValidation;

namespace MyFactory.Application.OldFeatures.Production.Commands.AllocateProductionOrder;

public sealed class AllocateProductionOrderCommandValidator : AbstractValidator<AllocateProductionOrderCommand>
{
    public AllocateProductionOrderCommandValidator()
    {
        RuleFor(command => command.ProductionOrderId)
            .NotEmpty();

        RuleFor(command => command.WorkshopId)
            .NotEmpty();

        RuleFor(command => command.QtyAllocated)
            .GreaterThan(0);
    }
}
