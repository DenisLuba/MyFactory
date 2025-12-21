using FluentValidation;

namespace MyFactory.Application.OldFeatures.Warehousing.Commands.CancelInventoryReceipt;

public sealed class CancelInventoryReceiptCommandValidator : AbstractValidator<CancelInventoryReceiptCommand>
{
    public CancelInventoryReceiptCommandValidator()
    {
        RuleFor(command => command.ReceiptId)
            .NotEmpty();
    }
}
