using FluentValidation;

namespace MyFactory.Application.Features.Warehousing.Commands.SubmitPurchaseRequest;

public sealed class SubmitPurchaseRequestCommandValidator : AbstractValidator<SubmitPurchaseRequestCommand>
{
    public SubmitPurchaseRequestCommandValidator()
    {
        RuleFor(command => command.PurchaseRequestId)
            .NotEmpty();
    }
}
