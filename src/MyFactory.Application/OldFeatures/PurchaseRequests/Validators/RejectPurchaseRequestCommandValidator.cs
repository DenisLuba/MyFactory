using FluentValidation;
using MyFactory.Application.Features.PurchaseRequests.Commands;

namespace MyFactory.Application.OldFeatures.PurchaseRequests.Validators;

public sealed class RejectPurchaseRequestCommandValidator : AbstractValidator<RejectPurchaseRequestCommand>
{
    public RejectPurchaseRequestCommandValidator()
    {
        RuleFor(command => command.PurchaseRequestId)
            .NotEmpty();
    }
}
