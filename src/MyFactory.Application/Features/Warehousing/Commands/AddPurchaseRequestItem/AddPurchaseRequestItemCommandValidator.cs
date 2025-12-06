using FluentValidation;

namespace MyFactory.Application.Features.Warehousing.Commands.AddPurchaseRequestItem;

public sealed class AddPurchaseRequestItemCommandValidator : AbstractValidator<AddPurchaseRequestItemCommand>
{
    public AddPurchaseRequestItemCommandValidator()
    {
        RuleFor(command => command.PurchaseRequestId)
            .NotEmpty();

        RuleFor(command => command.MaterialId)
            .NotEmpty();

        RuleFor(command => command.Quantity)
            .GreaterThan(0m);
    }
}
