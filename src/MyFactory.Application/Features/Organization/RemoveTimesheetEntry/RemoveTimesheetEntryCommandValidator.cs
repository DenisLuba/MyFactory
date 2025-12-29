using FluentValidation;

namespace MyFactory.Application.Features.RemoveTimesheetEntry;

public sealed class RemoveTimesheetEntryCommandValidator
    : AbstractValidator<RemoveTimesheetEntryCommand>
{
    public RemoveTimesheetEntryCommandValidator()
    {
        RuleFor(x => x.EntryId)
            .NotEmpty();
    }
}