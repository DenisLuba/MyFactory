using FluentValidation;

namespace MyFactory.Application.Features.Departments.CreateDepartment;

public sealed class CreateDepartmentCommandValidator
    : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(20)
            .Matches("^[A-Z0-9_-]+$")
            .WithMessage("Code must contain only uppercase letters, digits, '_' or '-'.");

        RuleFor(x => x.Type)
            .IsInEnum();
    }
}