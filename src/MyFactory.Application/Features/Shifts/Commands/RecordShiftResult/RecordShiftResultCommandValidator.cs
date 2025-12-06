using FluentValidation;

namespace MyFactory.Application.Features.Shifts.Commands.RecordShiftResult;

public sealed class RecordShiftResultCommandValidator : AbstractValidator<RecordShiftResultCommand>
{
    public RecordShiftResultCommandValidator()
    {
        RuleFor(command => command.ShiftPlanId)
            .NotEmpty();

        RuleFor(command => command.ActualQuantity)
            .GreaterThanOrEqualTo(0);

        RuleFor(command => command.HoursWorked)
            .GreaterThan(0);

        RuleFor(command => command.RecordedAt)
            .Must(recordedAt => recordedAt != default)
            .WithMessage("Recorded date is required.");
    }
}
