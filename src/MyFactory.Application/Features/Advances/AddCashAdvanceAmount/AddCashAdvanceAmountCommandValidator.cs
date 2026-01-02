using FluentValidation;

namespace MyFactory.Application.Features.Advances.AddCashAdvanceAmount;

public sealed class AddCashAdvanceAmountCommandValidator : AbstractValidator<AddCashAdvanceAmountCommand>
{
    public AddCashAdvanceAmountCommandValidator()
    {
        RuleFor(x => x.CashAdvanceId)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.IssueDate)
            .Must(d => d != default)
            .WithMessage("IssueDate must be specified.");
    }
}
