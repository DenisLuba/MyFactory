using FluentValidation;
using MyFactory.Application.Features.Finance.Commands.ProductionCostFacts;

namespace MyFactory.Application.OldFeatures.Finance.Validators;

public sealed class CalculateProductionCostFactCommandValidator : AbstractValidator<CalculateProductionCostFactCommand>
{
    public CalculateProductionCostFactCommandValidator()
    {
        RuleFor(command => command.PeriodMonth).InclusiveBetween(1, 12);
        RuleFor(command => command.PeriodYear).GreaterThanOrEqualTo(2000);
        RuleFor(command => command.SpecificationId).NotEmpty();
        RuleFor(command => command.QuantityProduced).GreaterThanOrEqualTo(0m);
        RuleFor(command => command.MaterialCost).GreaterThanOrEqualTo(0m);
        RuleFor(command => command.LaborCost).GreaterThanOrEqualTo(0m);
    }
}
