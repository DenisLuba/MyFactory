using FluentValidation;

namespace MyFactory.Application.Features.ExpenseTypes.CreateExpenseType;

public sealed class CreateExpenseTypeCommandValidator : AbstractValidator<CreateExpenseTypeCommand>
{
    public CreateExpenseTypeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
