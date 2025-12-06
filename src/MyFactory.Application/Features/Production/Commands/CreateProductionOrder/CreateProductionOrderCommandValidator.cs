using FluentValidation;

namespace MyFactory.Application.Features.Production.Commands.CreateProductionOrder;

public sealed class CreateProductionOrderCommandValidator : AbstractValidator<CreateProductionOrderCommand>
{
    public CreateProductionOrderCommandValidator()
    {
        RuleFor(command => command.OrderNumber)
            .NotEmpty();

        RuleFor(command => command.SpecificationId)
            .NotEmpty();

        RuleFor(command => command.QtyOrdered)
            .GreaterThan(0);
    }
}
