using FluentValidation;

namespace MyFactory.Application.Features.Employees.RemoveTimesheetEntry;

public sealed class RemoveTimesheetEntryCommandValidator
    : AbstractValidator<RemoveTimesheetEntryCommand>
{
    public RemoveTimesheetEntryCommandValidator()
    {
        RuleFor(x => x.EntryId)
            .NotEmpty();
    }
}