using FluentValidation;

namespace MyFactory.Application.Features.Departments.GetDepartmentDetails;

public sealed class GetDepartmentDetailsQueryValidator
    : AbstractValidator<GetDepartmentDetailsQuery>
{
    public GetDepartmentDetailsQueryValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .WithMessage("DepartmentId is required.");
    }
}

