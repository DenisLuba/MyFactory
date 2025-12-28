using FluentValidation;

namespace MyFactory.Application.Features.UpdateEmployee;

public sealed class UpdateEmployeeCommandValidator
    : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.PositionId)
            .NotEmpty();

        RuleFor(x => x.Grade)
            .GreaterThan(0);

        RuleFor(x => x.RatePerNormHour)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.PremiumPercent)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.HiredAt)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.Today);
    }
}
