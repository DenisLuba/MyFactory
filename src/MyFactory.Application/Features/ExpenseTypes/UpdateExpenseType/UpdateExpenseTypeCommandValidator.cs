using FluentValidation;

namespace MyFactory.Application.Features.ExpenseTypes.UpdateExpenseType;

public sealed class UpdateExpenseTypeCommandValidator : AbstractValidator<UpdateExpenseTypeCommand>
{
    public UpdateExpenseTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
