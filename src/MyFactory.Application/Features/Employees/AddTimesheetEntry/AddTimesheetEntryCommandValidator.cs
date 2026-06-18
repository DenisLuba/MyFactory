using FluentValidation;

namespace MyFactory.Application.Features.Employees.AddTimesheetEntry;

public sealed class AddTimesheetEntryCommandValidator
    : AbstractValidator<AddTimesheetEntryCommand>
{
    public AddTimesheetEntryCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.Date)
            .NotEqual(default(DateOnly))
            .WithMessage("Date must be specified.");

        RuleFor(x => x.Hours)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Comment)
            .MaximumLength(500);
    }
}
