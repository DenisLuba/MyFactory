using FluentValidation;

namespace MyFactory.Application.OldFeatures.Warehousing.Commands.ReserveInventory;

public sealed class ReserveInventoryCommandValidator : AbstractValidator<ReserveInventoryCommand>
{
    public ReserveInventoryCommandValidator()
    {
        RuleFor(command => command.WarehouseId)
            .NotEmpty();

        RuleFor(command => command.MaterialId)
            .NotEmpty();

        RuleFor(command => command.Quantity)
            .GreaterThan(0m);
    }
}
