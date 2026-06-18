using FluentValidation;

namespace MyFactory.Application.Features.Users.GetUsers;

public sealed class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        // No filters yet.
    }
}
