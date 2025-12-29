using FluentValidation;

namespace MyFactory.Application.Features.GetEmployeeProductionAssignments;

public sealed class GetEmployeeProductionAssignmentsQueryValidator
    : AbstractValidator<GetEmployeeProductionAssignmentsQuery>
{
    public GetEmployeeProductionAssignmentsQueryValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();
    }
}