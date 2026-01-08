using FluentValidation;

namespace MyFactory.Application.Features.Units.RemoveUnit;

public sealed class RemoveUnitCommandValidator : AbstractValidator<RemoveUnitCommand>
{
    public RemoveUnitCommandValidator()
    {
        RuleFor(x => x.UnitId).NotEmpty();
    }
}
