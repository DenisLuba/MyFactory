using FluentValidation;

namespace MyFactory.Application.Features.Advances.Commands.RejectAdvance;

public sealed class RejectAdvanceCommandValidator : AbstractValidator<RejectAdvanceCommand>
{
    public RejectAdvanceCommandValidator()
    {
        RuleFor(cmd => cmd.AdvanceId).NotEmpty();
    }
}
