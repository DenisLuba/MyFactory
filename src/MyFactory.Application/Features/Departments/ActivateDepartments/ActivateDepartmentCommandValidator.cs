using FluentValidation;

namespace MyFactory.Application.Features.Departments.ActivateDepartments;

public sealed class ActivateDepartmentsCommandValidator 
    : AbstractValidator<ActivateDepartmentCommand>
{
    public ActivateDepartmentsCommandValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty();
    }
}