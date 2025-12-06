using FluentValidation;
using MyFactory.Application.Features.PurchaseRequests.Commands;

namespace MyFactory.Application.Features.PurchaseRequests.Validators;

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
