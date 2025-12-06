using FluentValidation;

namespace MyFactory.Application.Features.Warehousing.Commands.CreateInventoryReceipt;

public sealed class CreateInventoryReceiptCommandValidator : AbstractValidator<CreateInventoryReceiptCommand>
{
    public CreateInventoryReceiptCommandValidator()
    {
        RuleFor(command => command.ReceiptNumber)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(command => command.SupplierId)
            .NotEmpty();

        RuleFor(command => command.ReceiptDate)
            .NotEmpty();
    }
}
