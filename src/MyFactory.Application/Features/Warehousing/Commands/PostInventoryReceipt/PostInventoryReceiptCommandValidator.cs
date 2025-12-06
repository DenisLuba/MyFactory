using FluentValidation;

namespace MyFactory.Application.Features.Warehousing.Commands.PostInventoryReceipt;

public sealed class PostInventoryReceiptCommandValidator : AbstractValidator<PostInventoryReceiptCommand>
{
    public PostInventoryReceiptCommandValidator()
    {
        RuleFor(command => command.ReceiptId)
            .NotEmpty();

        RuleFor(command => command.WarehouseId)
            .NotEmpty();
    }
}
