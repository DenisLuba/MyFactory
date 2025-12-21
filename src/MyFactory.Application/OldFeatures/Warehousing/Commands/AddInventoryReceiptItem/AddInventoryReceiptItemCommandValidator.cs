using FluentValidation;

namespace MyFactory.Application.OldFeatures.Warehousing.Commands.AddInventoryReceiptItem;

public sealed class AddInventoryReceiptItemCommandValidator : AbstractValidator<AddInventoryReceiptItemCommand>
{
    public AddInventoryReceiptItemCommandValidator()
    {
        RuleFor(command => command.ReceiptId)
            .NotEmpty();

        RuleFor(command => command.MaterialId)
            .NotEmpty();

        RuleFor(command => command.Quantity)
            .GreaterThan(0m);

        RuleFor(command => command.UnitPrice)
            .GreaterThan(0m);
    }
}
