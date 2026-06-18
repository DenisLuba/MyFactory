using FluentValidation;

namespace MyFactory.Application.Features.MaterialTypes.UpdateMaterialType;

public sealed class UpdateMaterialTypeCommandValidator : AbstractValidator<UpdateMaterialTypeCommand>
{
    public UpdateMaterialTypeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
