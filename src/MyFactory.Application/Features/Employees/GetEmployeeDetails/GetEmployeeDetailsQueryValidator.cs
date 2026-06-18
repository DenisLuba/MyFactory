using FluentValidation;

namespace MyFactory.Application.Features.Employees.GetEmployeeDetails;

public sealed class GetEmployeeDetailsQueryValidator
    : AbstractValidator<GetEmployeeDetailsQuery>
{
    public GetEmployeeDetailsQueryValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();
    }
}