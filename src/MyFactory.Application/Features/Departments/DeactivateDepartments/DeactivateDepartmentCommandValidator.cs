using FluentValidation;

namespace MyFactory.Application.Features.Departments.DeactivateDepartments;

public sealed class DeactivateDepartmentCommandValidator 
    : AbstractValidator<DeactivateDepartmentCommand>
{
    public DeactivateDepartmentCommandValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty();
    }
}