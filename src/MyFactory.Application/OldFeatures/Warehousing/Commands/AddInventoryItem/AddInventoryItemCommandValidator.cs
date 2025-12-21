using FluentValidation;

namespace MyFactory.Application.OldFeatures.Warehousing.Commands.AddInventoryItem;

public sealed class AddInventoryItemCommandValidator : AbstractValidator<AddInventoryItemCommand>
{
    public AddInventoryItemCommandValidator()
    {
        RuleFor(command => command.WarehouseId)
            .NotEmpty();

        RuleFor(command => command.MaterialId)
            .NotEmpty();
    }
}
