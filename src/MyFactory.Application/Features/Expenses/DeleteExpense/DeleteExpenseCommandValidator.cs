using FluentValidation;

namespace MyFactory.Application.Features.Expenses.DeleteExpense;

public sealed class DeleteExpenseCommandValidator : AbstractValidator<DeleteExpenseCommand>
{
    public DeleteExpenseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
