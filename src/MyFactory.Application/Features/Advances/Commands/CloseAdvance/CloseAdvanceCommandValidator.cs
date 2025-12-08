using FluentValidation;

namespace MyFactory.Application.Features.Advances.Commands.CloseAdvance;

public sealed class CloseAdvanceCommandValidator : AbstractValidator<CloseAdvanceCommand>
{
    public CloseAdvanceCommandValidator()
    {
        RuleFor(cmd => cmd.AdvanceId).NotEmpty();
    }
}
