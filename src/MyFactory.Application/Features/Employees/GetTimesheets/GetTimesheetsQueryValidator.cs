using FluentValidation;

namespace MyFactory.Application.Features.Employees.GetTimesheets;

public sealed class GetTimesheetsQueryValidator
    : AbstractValidator<GetTimesheetsQuery>
{
    public GetTimesheetsQueryValidator()
    {
        RuleFor(x => x.Period.Year)
            .InclusiveBetween(2000, 2100);

        RuleFor(x => x.Period.Month)
            .InclusiveBetween(1, 12);

        RuleFor(x => x)
            .Must(x => x.EmployeeId.HasValue || x.DepartmentId.HasValue)
            .WithMessage("Either EmployeeId or DepartmentId must be specified.");
    }
}