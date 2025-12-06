using FluentValidation;

namespace MyFactory.Application.Features.Warehousing.Commands.RejectPurchaseRequest;

public sealed class RejectPurchaseRequestCommandValidator : AbstractValidator<RejectPurchaseRequestCommand>
{
    public RejectPurchaseRequestCommandValidator()
    {
        RuleFor(command => command.PurchaseRequestId)
            .NotEmpty();
    }
}
