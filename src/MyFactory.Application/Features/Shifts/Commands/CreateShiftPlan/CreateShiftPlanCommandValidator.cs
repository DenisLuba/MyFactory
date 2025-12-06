using FluentValidation;

namespace MyFactory.Application.Features.Shifts.Commands.CreateShiftPlan;

public sealed class CreateShiftPlanCommandValidator : AbstractValidator<CreateShiftPlanCommand>
{
    public CreateShiftPlanCommandValidator()
    {
        RuleFor(command => command.EmployeeId)
            .NotEmpty();

        RuleFor(command => command.SpecificationId)
            .NotEmpty();

        RuleFor(command => command.ShiftDate)
            .Must(date => date != default)
            .WithMessage("Shift date is required.");

        RuleFor(command => command.ShiftType)
            .NotEmpty();

        RuleFor(command => command.PlannedQuantity)
            .GreaterThan(0);
    }
}
