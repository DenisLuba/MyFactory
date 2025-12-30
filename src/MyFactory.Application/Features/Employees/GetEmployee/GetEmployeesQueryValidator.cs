using FluentValidation;

namespace MyFactory.Application.Features.Organization.Employees.GetEmployee;

public sealed class GetEmployeesQueryValidator
    : AbstractValidator<GetEmployeesQuery>
{
    public GetEmployeesQueryValidator()
    {
        RuleFor(x => x.Search)
            .MaximumLength(200)
            .WithMessage("Search text is too long.");

        RuleFor(x => x.SortBy)
            .IsInEnum()
            .WithMessage("Invalid sort field.");
    }
}