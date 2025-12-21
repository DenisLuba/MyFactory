using FluentValidation;

namespace MyFactory.Application.Features.Materials.UpdateMaterial;

public sealed class UpdateMaterialCommandValidator
    : AbstractValidator<UpdateMaterialCommand>
{
    public UpdateMaterialCommandValidator()
    {
        RuleFor(x => x.MaterialId).NotEmpty();

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


