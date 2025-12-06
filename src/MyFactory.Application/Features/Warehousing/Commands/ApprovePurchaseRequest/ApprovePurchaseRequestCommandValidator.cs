using FluentValidation;

namespace MyFactory.Application.Features.Warehousing.Commands.ApprovePurchaseRequest;

public sealed class ApprovePurchaseRequestCommandValidator : AbstractValidator<ApprovePurchaseRequestCommand>
{
    public ApprovePurchaseRequestCommandValidator()
    {
        RuleFor(command => command.PurchaseRequestId)
            .NotEmpty();
    }
}
