using FluentValidation;

namespace MyFactory.Application.Features.Advances.CreateCashAdvanceReturn;

public sealed class CreateCashAdvanceReturnCommandValidator : AbstractValidator<CreateCashAdvanceReturnCommand>
{
    public CreateCashAdvanceReturnCommandValidator()
    {
        RuleFor(x => x.CashAdvanceId)
            .NotEmpty();

        RuleFor(x => x.ReturnDate)
            .Must(d => d != default)
            .WithMessage("ReturnDate must be specified.");

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Description)
            .MaximumLength(2000);
    }
}
