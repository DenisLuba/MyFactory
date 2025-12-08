using FluentValidation;
using MyFactory.Application.Features.Finance.Commands.ExpenseTypes;

namespace MyFactory.Application.Features.Finance.Validators;

public sealed class UpdateExpenseTypeCommandValidator : AbstractValidator<UpdateExpenseTypeCommand>
{
    public UpdateExpenseTypeCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty();
        RuleFor(command => command.Name).NotEmpty();
        RuleFor(command => command.Category).NotEmpty();
    }
}
