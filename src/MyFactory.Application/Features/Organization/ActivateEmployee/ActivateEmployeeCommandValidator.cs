using FluentValidation;

namespace MyFactory.Application.Features.ActivateEmployee;

public sealed class ActivateEmployeeCommandValidator
    : AbstractValidator<ActivateEmployeeCommand>
{
    public ActivateEmployeeCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.HiredAt)
            .NotEqual(default(DateTime))
            .WithMessage("HiredAt must be specified.");
    }
}