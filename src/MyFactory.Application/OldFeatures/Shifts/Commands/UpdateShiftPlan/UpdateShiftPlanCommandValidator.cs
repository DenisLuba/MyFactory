using FluentValidation;

namespace MyFactory.Application.OldFeatures.Shifts.Commands.UpdateShiftPlan;

public sealed class UpdateShiftPlanCommandValidator : AbstractValidator<UpdateShiftPlanCommand>
{
    public UpdateShiftPlanCommandValidator()
    {
        RuleFor(command => command.ShiftPlanId)
            .NotEmpty();

        RuleFor(command => command.ShiftType)
            .NotEmpty();

        RuleFor(command => command.PlannedQuantity)
            .GreaterThan(0);
    }
}
