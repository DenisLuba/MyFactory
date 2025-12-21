using FluentValidation;

namespace MyFactory.Application.OldFeatures.Advances.Commands.RejectAdvance;

public sealed class RejectAdvanceCommandValidator : AbstractValidator<RejectAdvanceCommand>
{
    public RejectAdvanceCommandValidator()
    {
        RuleFor(cmd => cmd.AdvanceId).NotEmpty();
    }
}
