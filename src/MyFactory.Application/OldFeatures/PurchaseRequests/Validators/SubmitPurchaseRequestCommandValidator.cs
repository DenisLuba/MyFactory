using FluentValidation;
using MyFactory.Application.Features.PurchaseRequests.Commands;

namespace MyFactory.Application.OldFeatures.PurchaseRequests.Validators;

public sealed class SubmitPurchaseRequestCommandValidator : AbstractValidator<SubmitPurchaseRequestCommand>
{
    public SubmitPurchaseRequestCommandValidator()
    {
        RuleFor(command => command.PurchaseRequestId)
            .NotEmpty();
    }
}
