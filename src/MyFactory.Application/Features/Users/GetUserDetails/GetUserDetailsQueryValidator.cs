using FluentValidation;

namespace MyFactory.Application.Features.Users.GetUserDetails;

public sealed class GetUserDetailsQueryValidator : AbstractValidator<GetUserDetailsQuery>
{
    public GetUserDetailsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
