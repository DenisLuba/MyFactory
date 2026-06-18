using FluentValidation;

namespace MyFactory.Application.Features.MaterialTypes.DeactivateMaterialType;

public sealed class DeactivateMaterialTypeCommandValidator : AbstractValidator<DeactivateMaterialTypeCommand>
{
    public DeactivateMaterialTypeCommandValidator()
    {
        RuleFor(x => x.MaterialTypeId).NotEmpty();
    }
}
