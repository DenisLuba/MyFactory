using FluentValidation;

namespace MyFactory.Application.OldFeatures.Specifications.Commands.AddBomItem;

public sealed class AddSpecificationBomItemCommandValidator : AbstractValidator<AddSpecificationBomItemCommand>
{
    public AddSpecificationBomItemCommandValidator()
    {
        RuleFor(command => command.SpecificationId)
            .NotEmpty();

        RuleFor(command => command.MaterialId)
            .NotEmpty();

        RuleFor(command => command.Quantity)
            .GreaterThan(0);

        RuleFor(command => command.Unit)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(command => command.UnitCost)
            .GreaterThan(0)
            .When(command => command.UnitCost.HasValue);
    }
}
