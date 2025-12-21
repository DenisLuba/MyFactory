using FluentValidation;
using MyFactory.Application.Features.Finance.Commands.ExpenseTypes;

namespace MyFactory.Application.OldFeatures.Finance.Validators;

public sealed class CreateExpenseTypeCommandValidator : AbstractValidator<CreateExpenseTypeCommand>
{
    public CreateExpenseTypeCommandValidator()
    {
        RuleFor(command => command.Name).NotEmpty();
        RuleFor(command => command.Category).NotEmpty();
    }
}
