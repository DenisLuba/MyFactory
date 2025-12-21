using FluentValidation;
using MyFactory.Application.Features.PurchaseRequests.Commands;

namespace MyFactory.Application.OldFeatures.PurchaseRequests.Validators;

public sealed class UpdatePurchaseRequestItemCommandValidator : AbstractValidator<UpdatePurchaseRequestItemCommand>
{
    public UpdatePurchaseRequestItemCommandValidator()
    {
        RuleFor(command => command.PurchaseRequestId)
            .NotEmpty();

        RuleFor(command => command.PurchaseRequestItemId)
            .NotEmpty();

        RuleFor(command => command.MaterialId)
            .NotEmpty();

        RuleFor(command => command.Quantity)
            .GreaterThan(0);
    }
}
