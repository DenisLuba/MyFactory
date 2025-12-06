using FluentValidation;

namespace MyFactory.Application.Features.Warehousing.Commands.CancelInventoryReceipt;

public sealed class CancelInventoryReceiptCommandValidator : AbstractValidator<CancelInventoryReceiptCommand>
{
    public CancelInventoryReceiptCommandValidator()
    {
        RuleFor(command => command.ReceiptId)
            .NotEmpty();
    }
}
