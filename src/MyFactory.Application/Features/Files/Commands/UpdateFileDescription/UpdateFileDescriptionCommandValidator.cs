using FluentValidation;
using MyFactory.Domain.Entities.Files;

namespace MyFactory.Application.Features.Files.Commands.UpdateFileDescription;

public sealed class UpdateFileDescriptionCommandValidator : AbstractValidator<UpdateFileDescriptionCommand>
{
    public UpdateFileDescriptionCommandValidator()
    {
        RuleFor(cmd => cmd.FileId).NotEmpty();
        RuleFor(cmd => cmd.Description)
            .NotEmpty()
            .MaximumLength(FileResource.DescriptionMaxLength);
    }
}
