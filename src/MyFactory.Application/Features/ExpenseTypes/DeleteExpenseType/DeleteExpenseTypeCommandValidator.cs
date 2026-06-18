using FluentValidation;

namespace MyFactory.Application.Features.ExpenseTypes.DeleteExpenseType;

public sealed class DeleteExpenseTypeCommandValidator : AbstractValidator<DeleteExpenseTypeCommand>
{
    public DeleteExpenseTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
