using FluentValidation;
using MyFactory.Application.Features.PurchaseRequests.Commands;

namespace MyFactory.Application.OldFeatures.PurchaseRequests.Validators;

public sealed class RemovePurchaseRequestItemCommandValidator : AbstractValidator<RemovePurchaseRequestItemCommand>
{
    public RemovePurchaseRequestItemCommandValidator()
    {
        RuleFor(command => command.PurchaseRequestId)
            .NotEmpty();

        RuleFor(command => command.PurchaseRequestItemId)
            .NotEmpty();
    }
}
