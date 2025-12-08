using FluentValidation;

namespace MyFactory.Application.Features.Files.Commands.DeleteFile;

public sealed class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
{
    public DeleteFileCommandValidator()
    {
        RuleFor(cmd => cmd.FileId).NotEmpty();
    }
}
