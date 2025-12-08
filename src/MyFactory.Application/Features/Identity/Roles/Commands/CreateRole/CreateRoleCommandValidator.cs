using FluentValidation;
using MyFactory.Domain.Entities.Identity;

namespace MyFactory.Application.Features.Identity.Roles.Commands.CreateRole;

public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    private const int NameMaxLength = 256;

    public CreateRoleCommandValidator()
    {
        RuleFor(cmd => cmd.Name)
            .NotEmpty()
            .MaximumLength(NameMaxLength);

        RuleFor(cmd => cmd.Description)
            .MaximumLength(Role.DescriptionMaxLength)
            .When(cmd => !string.IsNullOrWhiteSpace(cmd.Description));
    }
}
