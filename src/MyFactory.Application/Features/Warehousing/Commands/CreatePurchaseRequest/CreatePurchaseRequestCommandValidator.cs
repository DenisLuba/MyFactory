using FluentValidation;

namespace MyFactory.Application.Features.Warehousing.Commands.CreatePurchaseRequest;

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
