using FluentValidation;

namespace MyFactory.Application.OldFeatures.Materials.Commands.CreateMaterial;

public sealed class CreateMaterialCommandValidator : AbstractValidator<CreateMaterialCommand>
{
    public CreateMaterialCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(command => command.Unit)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(command => command.MaterialTypeId)
            .NotEmpty();
    }
}
