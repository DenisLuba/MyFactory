using FluentValidation;

namespace MyFactory.Application.Features.Positions.UpdatePosition;

public sealed class UpdatePositionCommandValidator
    : AbstractValidator<UpdatePositionCommand>
{
    public UpdatePositionCommandValidator()
    {
        RuleFor(x => x.PositionId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.DepartmentId)
            .NotEmpty();

        RuleFor(x => x.Code)
            .MaximumLength(50);

        RuleFor(x => x.BaseNormPerHour)
            .GreaterThan(0)
            .When(x => x.BaseNormPerHour.HasValue);

        RuleFor(x => x.BaseRatePerNormHour)
            .GreaterThan(0)
            .When(x => x.BaseRatePerNormHour.HasValue);

        RuleFor(x => x.DefaultPremiumPercent)
            .GreaterThanOrEqualTo(0)
            .When(x => x.DefaultPremiumPercent.HasValue);
    }
}
