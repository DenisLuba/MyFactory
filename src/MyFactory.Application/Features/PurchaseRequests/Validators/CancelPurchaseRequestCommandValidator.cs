using FluentValidation;
using MyFactory.Application.Features.PurchaseRequests.Commands;

namespace MyFactory.Application.Features.PurchaseRequests.Validators;

public sealed class CancelPurchaseRequestCommandValidator : AbstractValidator<CancelPurchaseRequestCommand>
{
    public CancelPurchaseRequestCommandValidator()
    {
        RuleFor(command => command.PurchaseRequestId)
            .NotEmpty();
    }
}
