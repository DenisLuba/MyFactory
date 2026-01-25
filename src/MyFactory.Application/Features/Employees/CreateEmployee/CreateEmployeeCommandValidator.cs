using FluentValidation;

namespace MyFactory.Application.Features.Employees.CreateEmployee;

public sealed class CreateEmployeeCommandValidator
    : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.PositionId)
            .NotEmpty();

        RuleFor(x => x.Grade)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.RatePerNormHour)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.PremiumPercent)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.HiredAt)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.Today);

        RuleFor(x => x)
            .Must(x => x.IsActive || x.HiredAt <= DateTime.Today)
            .WithMessage("Inactive employee must have valid hire date.");
    }
}
