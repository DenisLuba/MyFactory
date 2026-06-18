using FluentValidation;

namespace MyFactory.Application.Features.Advances.CreateCashAdvanceExpense;

public sealed class CreateCashAdvanceExpenseCommandValidator : AbstractValidator<CreateCashAdvanceExpenseCommand>
{
    public CreateCashAdvanceExpenseCommandValidator()
    {
        RuleFor(x => x.CashAdvanceId)
            .NotEmpty();

        RuleFor(x => x.ExpenseDate)
            .Must(d => d != default)
            .WithMessage("ExpenseDate must be specified.");

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Description)
            .MaximumLength(2000);
    }
}
