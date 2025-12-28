using FluentValidation;

namespace MyFactory.Application.Features.DeactivateEmployee;

public sealed class DeactivateEmployeeCommandValidator
    : AbstractValidator<DeactivateEmployeeCommand>
{
    public DeactivateEmployeeCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.FiredAt)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow);
    }
}