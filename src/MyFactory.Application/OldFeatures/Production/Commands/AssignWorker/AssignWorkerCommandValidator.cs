using FluentValidation;

namespace MyFactory.Application.OldFeatures.Production.Commands.AssignWorker;

public sealed class AssignWorkerCommandValidator : AbstractValidator<AssignWorkerCommand>
{
    public AssignWorkerCommandValidator()
    {
        RuleFor(command => command.StageId)
            .NotEmpty();

        RuleFor(command => command.EmployeeId)
            .NotEmpty();

        RuleFor(command => command.QtyAssigned)
            .GreaterThan(0);

        RuleFor(command => command.QtyCompleted)
            .GreaterThanOrEqualTo(0);

        RuleFor(command => command)
            .Must(command => command.QtyCompleted <= command.QtyAssigned)
            .WithMessage("Completed quantity cannot exceed assigned quantity.");
    }
}
