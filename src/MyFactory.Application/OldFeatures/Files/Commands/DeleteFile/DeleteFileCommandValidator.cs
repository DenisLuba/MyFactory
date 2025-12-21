using FluentValidation;

namespace MyFactory.Application.OldFeatures.Files.Commands.DeleteFile;

public sealed class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
{
    public DeleteFileCommandValidator()
    {
        RuleFor(cmd => cmd.FileId).NotEmpty();
    }
}
