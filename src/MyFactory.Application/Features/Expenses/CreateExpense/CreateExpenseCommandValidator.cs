using FluentValidation;

namespace MyFactory.Application.Features.Expenses.CreateExpense;

public sealed class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
{
    public CreateExpenseCommandValidator()
    {
        RuleFor(x => x.ExpenseTypeId)
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
