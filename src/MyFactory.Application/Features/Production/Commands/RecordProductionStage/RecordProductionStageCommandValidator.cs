using System;
using FluentValidation;

namespace MyFactory.Application.Features.Production.Commands.RecordProductionStage;

public sealed class RecordProductionStageCommandValidator : AbstractValidator<RecordProductionStageCommand>
{
    public RecordProductionStageCommandValidator()
    {
        RuleFor(command => command.ProductionOrderId)
            .NotEmpty();

        RuleFor(command => command.WorkshopId)
            .NotEmpty();

        RuleFor(command => command.StageType)
            .NotEmpty();

        RuleFor(command => command.QtyIn)
            .GreaterThan(0);

        RuleFor(command => command.QtyOut)
            .GreaterThanOrEqualTo(0);

        RuleFor(command => command.RecordedAt)
            .NotEmpty()
            .LessThanOrEqualTo(_ => DateTime.UtcNow);

        RuleFor(command => command)
            .Must(command => command.QtyOut <= command.QtyIn)
            .WithMessage("Output quantity cannot exceed input quantity.");
    }
}
