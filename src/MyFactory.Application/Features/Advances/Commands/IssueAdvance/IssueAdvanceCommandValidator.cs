using FluentValidation;

namespace MyFactory.Application.Features.Advances.Commands.IssueAdvance;

public sealed class IssueAdvanceCommandValidator : AbstractValidator<IssueAdvanceCommand>
{
    public IssueAdvanceCommandValidator()
    {
        RuleFor(cmd => cmd.EmployeeId).NotEmpty();
        RuleFor(cmd => cmd.Amount).GreaterThan(0);
        RuleFor(cmd => cmd.IssuedAt).Must(date => date != default)
            .WithMessage("Issued date is required.");
    }
}
