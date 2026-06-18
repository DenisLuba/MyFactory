using FluentValidation;

namespace MyFactory.Application.Features.Advances.CreateCashAdvance;

public sealed class CreateCashAdvanceCommandValidator : AbstractValidator<CreateCashAdvanceCommand>
{
    public CreateCashAdvanceCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.IssueDate)
            .Must(d => d != default)
            .WithMessage("IssueDate must be specified.");

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Description)
            .MaximumLength(2000);
    }
}
