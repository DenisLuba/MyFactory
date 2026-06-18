using FluentValidation;

namespace MyFactory.Application.Features.Units.AddUnit;

public sealed class AddUnitCommandValidator : AbstractValidator<AddUnitCommand>
{
    public AddUnitCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
    }
}
