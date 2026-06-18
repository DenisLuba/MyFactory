using FluentValidation;

namespace MyFactory.Application.Features.Materials.RemoveMaterial;

public sealed class RemoveMaterialCommandValidator : AbstractValidator<RemoveMaterialCommand>
{
    public RemoveMaterialCommandValidator()
    {
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
