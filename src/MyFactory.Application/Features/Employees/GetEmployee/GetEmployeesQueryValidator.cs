using FluentValidation;

namespace MyFactory.Application.Features.Employees.GetEmployee;

public sealed class GetEmployeesQueryValidator : AbstractValidator<GetEmployeesQuery>
{
    private const int MaxTake = 100;
    private static readonly string[] AllowedSortFields =
    [
        "fullname",
        "department",
        "position"
    ];

    public GetEmployeesQueryValidator()
    {
        RuleFor(x => x.FullName)
            .MaximumLength(200);

        RuleFor(x => x.SortBy)
            .Must(sort =>
                sort is null ||
                AllowedSortFields.Contains(sort.Trim().ToLowerInvariant()))
            .WithMessage("SortBy must be one of: fullname, department or position.");

        RuleFor(x => x.Skip)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Take)
            .InclusiveBetween(1, MaxTake);

        RuleFor(x => x.Grade)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Grade.HasValue);

        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .When(x => x.DepartmentId.HasValue);

        RuleFor(x => x.PositionId)
            .NotEmpty()
            .When(x => x.PositionId.HasValue);
    }
}
