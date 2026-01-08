using FluentValidation;

namespace MyFactory.Application.Features.Materials.CreateMaterial;

public sealed class CreateMaterialCommandValidator : AbstractValidator<CreateMaterialCommand>
{
    public CreateMaterialCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.MaterialTypeId)
            .NotEmpty();

        RuleFor(x => x.UnitId)
            .NotEmpty();

        RuleFor(x => x.Color)
            .MaximumLength(100);
    }
}
