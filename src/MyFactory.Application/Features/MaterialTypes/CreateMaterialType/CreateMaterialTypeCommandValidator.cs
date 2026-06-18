using FluentValidation;

namespace MyFactory.Application.Features.MaterialTypes.CreateMaterialType;

public sealed class CreateMaterialTypeCommandValidator : AbstractValidator<CreateMaterialTypeCommand>
{
    public CreateMaterialTypeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
