using FluentValidation;

namespace MyFactory.Application.Features.Expenses.UpdateExpense;

public sealed class UpdateExpenseCommandValidator : AbstractValidator<UpdateExpenseCommand>
{
    public UpdateExpenseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

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
