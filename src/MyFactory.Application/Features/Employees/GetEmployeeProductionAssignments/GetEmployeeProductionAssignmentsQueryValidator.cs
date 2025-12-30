using FluentValidation;

namespace MyFactory.Application.Features.Employees.GetEmployeeProductionAssignments;

public sealed class GetEmployeeProductionAssignmentsQueryValidator
    : AbstractValidator<GetEmployeeProductionAssignmentsQuery>
{
    public GetEmployeeProductionAssignmentsQueryValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();
    }
}