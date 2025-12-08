using FluentValidation;

namespace MyFactory.Application.Features.Advances.Commands.ApproveAdvance;

public sealed class ApproveAdvanceCommandValidator : AbstractValidator<ApproveAdvanceCommand>
{
    public ApproveAdvanceCommandValidator()
    {
        RuleFor(cmd => cmd.AdvanceId).NotEmpty();
    }
}
