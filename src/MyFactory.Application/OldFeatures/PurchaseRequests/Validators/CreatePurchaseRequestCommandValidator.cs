using FluentValidation;
using MyFactory.Application.Features.PurchaseRequests.Commands;

namespace MyFactory.Application.OldFeatures.PurchaseRequests.Validators;

public sealed class CreatePurchaseRequestCommandValidator : AbstractValidator<CreatePurchaseRequestCommand>
{
    public CreatePurchaseRequestCommandValidator()
    {
        RuleFor(command => command.PrNumber)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(command => command.CreatedAt)
            .NotEmpty();
    }
}
