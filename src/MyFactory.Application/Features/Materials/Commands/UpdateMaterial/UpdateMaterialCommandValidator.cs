using System;
using FluentValidation;

namespace MyFactory.Application.Features.Materials.Commands.UpdateMaterial;

public sealed class UpdateMaterialCommandValidator : AbstractValidator<UpdateMaterialCommand>
{
    public UpdateMaterialCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

        RuleFor(command => command)
            .Must(command => command.Name is not null
                || command.Unit is not null
                || command.MaterialTypeId.HasValue
                || command.IsActive.HasValue)
            .WithMessage("At least one field must be provided for update.");

        When(command => command.Name is not null, () =>
        {
            RuleFor(command => command.Name!)
                .NotEmpty()
                .MaximumLength(256);
        });

        When(command => command.Unit is not null, () =>
        {
            RuleFor(command => command.Unit!)
                .NotEmpty()
                .MaximumLength(64);
        });

        When(command => command.MaterialTypeId.HasValue, () =>
        {
            RuleFor(command => command.MaterialTypeId)
                .Must(id => id.HasValue && id.Value != Guid.Empty)
                .WithMessage("Material type id must be provided.");
        });
    }
}
