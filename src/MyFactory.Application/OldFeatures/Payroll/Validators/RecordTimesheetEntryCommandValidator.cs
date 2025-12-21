using FluentValidation;
using MyFactory.Application.Features.Payroll.Commands.RecordTimesheetEntry;

namespace MyFactory.Application.OldFeatures.Payroll.Validators;

public sealed class RecordTimesheetEntryCommandValidator : AbstractValidator<RecordTimesheetEntryCommand>
{
    public RecordTimesheetEntryCommandValidator()
    {
        RuleFor(command => command.EmployeeId).NotEmpty();
        RuleFor(command => command.WorkDate)
            .Must(date => date != default)
            .WithMessage("Work date is required.");
        RuleFor(command => command.HoursWorked)
            .GreaterThanOrEqualTo(0m)
            .WithMessage("Hours cannot be negative.");
    }
}
