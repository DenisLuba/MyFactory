using FluentValidation;

namespace MyFactory.Application.Features.Users.RemoveUser;

public sealed class RemoveUserCommandValidator : AbstractValidator<RemoveUserCommand>
{
    public RemoveUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
