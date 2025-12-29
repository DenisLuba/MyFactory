using FluentValidation;

namespace MyFactory.Application.Features.GetEmployeeDetails;

public sealed class GetEmployeeDetailsQueryValidator
    : AbstractValidator<GetEmployeeDetailsQuery>
{
    public GetEmployeeDetailsQueryValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();
    }
}