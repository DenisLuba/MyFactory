using FluentValidation;

namespace MyFactory.Application.Features.Positions.CreatePositions;

public sealed class CreatePositionCommandValidator
    : AbstractValidator<CreatePositionCommand>
{
    public CreatePositionCommandValidator()
    {
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
