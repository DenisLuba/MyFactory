using FluentValidation;

namespace MyFactory.Application.Features.Employees.UpdateTimesheetEntry;

public sealed class UpdateTimesheetEntryCommandValidator
    : AbstractValidator<UpdateTimesheetEntryCommand>
{
    public UpdateTimesheetEntryCommandValidator()
    {
        RuleFor(x => x.EntryId)
            .NotEmpty();

        RuleFor(x => x.Hours)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Comment)
            .MaximumLength(500);
    }
}