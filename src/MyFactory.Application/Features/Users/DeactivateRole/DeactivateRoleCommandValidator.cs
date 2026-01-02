using FluentValidation;

namespace MyFactory.Application.Features.Users.DeactivateRole;

public sealed class DeactivateRoleCommandValidator : AbstractValidator<DeactivateRoleCommand>
{
    public DeactivateRoleCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty();
    }
}
