using FluentValidation;

namespace MyFactory.Application.Features.Authentication.RegisterUser;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Password)
            .NotEmpty();

        RuleFor(x => x.RoleId)
            .NotEmpty();
    }
}
