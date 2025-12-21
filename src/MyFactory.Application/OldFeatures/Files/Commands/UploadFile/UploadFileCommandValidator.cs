using FluentValidation;
using MyFactory.Domain.Entities.Files;

namespace MyFactory.Application.OldFeatures.Files.Commands.UploadFile;

public sealed class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator()
    {
        RuleFor(cmd => cmd.FileName).NotEmpty();
        RuleFor(cmd => cmd.StoragePath).NotEmpty();
        RuleFor(cmd => cmd.ContentType).NotEmpty();
        RuleFor(cmd => cmd.SizeBytes).GreaterThanOrEqualTo(0);
        RuleFor(cmd => cmd.UploadedByUserId).NotEmpty();
        RuleFor(cmd => cmd.Description)
            .MaximumLength(FileResource.DescriptionMaxLength);
        RuleFor(cmd => cmd.UploadedAt)
            .Must(date => !date.HasValue || date.Value != default)
            .WithMessage("Uploaded timestamp is required when specified.");
    }
}
