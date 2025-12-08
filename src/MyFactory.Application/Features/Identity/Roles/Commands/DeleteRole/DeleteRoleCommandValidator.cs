using FluentValidation;

namespace MyFactory.Application.Features.Identity.Roles.Commands.DeleteRole;

public sealed class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(cmd => cmd.RoleId).NotEmpty();
    }
}
