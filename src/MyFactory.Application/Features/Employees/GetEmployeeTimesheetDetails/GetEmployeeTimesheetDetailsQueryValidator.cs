using FluentValidation;

namespace MyFactory.Application.Features.Employees.GetEmployeeTimesheetDetails;

public sealed class GetEmployeeTimesheetDetailsQueryValidator
    : AbstractValidator<GetEmployeeTimesheetDetailsQuery>
{
    public GetEmployeeTimesheetDetailsQueryValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.Period.Year)
            .InclusiveBetween(2000, 2100);

        RuleFor(x => x.Period.Month)
            .InclusiveBetween(1, 12);
    }
}