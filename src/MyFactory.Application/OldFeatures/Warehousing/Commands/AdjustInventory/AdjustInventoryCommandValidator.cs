using FluentValidation;

namespace MyFactory.Application.OldFeatures.Warehousing.Commands.AdjustInventory;

public sealed class AdjustInventoryCommandValidator : AbstractValidator<AdjustInventoryCommand>
{
    public AdjustInventoryCommandValidator()
    {
        RuleFor(command => command.WarehouseId)
            .NotEmpty();

        RuleFor(command => command.MaterialId)
            .NotEmpty();

        RuleFor(command => command.QuantityDelta)
            .NotEqual(0m)
            .WithMessage("Quantity delta must be non-zero.");

        When(command => command.QuantityDelta > 0, () =>
        {
            RuleFor(command => command.UnitPrice)
                .NotNull()
                .GreaterThan(0m);
        });
    }
}
