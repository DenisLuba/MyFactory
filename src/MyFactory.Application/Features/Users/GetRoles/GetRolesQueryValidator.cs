using FluentValidation;

namespace MyFactory.Application.Features.Users.GetRoles;

public sealed class GetRolesQueryValidator : AbstractValidator<GetRolesQuery>
{
    public GetRolesQueryValidator()
    {
        // No filters for now.
    }
}
