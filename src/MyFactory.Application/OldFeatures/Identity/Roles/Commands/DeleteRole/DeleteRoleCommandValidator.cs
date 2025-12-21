using FluentValidation;

namespace MyFactory.Application.OldFeatures.Identity.Roles.Commands.DeleteRole;

public sealed class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(cmd => cmd.RoleId).NotEmpty();
    }
}
