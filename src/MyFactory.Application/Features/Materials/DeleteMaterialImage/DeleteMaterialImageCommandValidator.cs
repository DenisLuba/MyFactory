using FluentValidation;

namespace MyFactory.Application.Features.Materials.DeleteMaterialImage;

public sealed class DeleteMaterialImageCommandValidator : AbstractValidator<DeleteMaterialImageCommand>
{
    public DeleteMaterialImageCommandValidator()
    {
        RuleFor(x => x.ImageId).NotEmpty();
    }
}
