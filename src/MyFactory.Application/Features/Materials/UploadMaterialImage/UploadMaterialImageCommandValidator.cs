using FluentValidation;

namespace MyFactory.Application.Features.Materials.UploadMaterialImage;

public sealed class UploadMaterialImageCommandValidator : AbstractValidator<UploadMaterialImageCommand>
{
    public UploadMaterialImageCommandValidator()
    {
        RuleFor(x => x.MaterialId).NotEmpty();
        RuleFor(x => x.FileName).NotEmpty();
        RuleFor(x => x.Content).NotNull().Must(c => c.Length > 0).WithMessage("File content is required.");
    }
}
