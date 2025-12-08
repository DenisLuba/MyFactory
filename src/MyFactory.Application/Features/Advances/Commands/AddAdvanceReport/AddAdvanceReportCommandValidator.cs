using FluentValidation;

namespace MyFactory.Application.Features.Advances.Commands.AddAdvanceReport;

public sealed class AddAdvanceReportCommandValidator : AbstractValidator<AddAdvanceReportCommand>
{
    public AddAdvanceReportCommandValidator()
    {
        RuleFor(cmd => cmd.AdvanceId).NotEmpty();
        RuleFor(cmd => cmd.Description).NotEmpty().MaximumLength(512);
        RuleFor(cmd => cmd.Amount).GreaterThan(0);
        RuleFor(cmd => cmd.FileId).NotEmpty();
        RuleFor(cmd => cmd.SpentAt).Must(date => date != default)
            .WithMessage("Spent date is required.");
    }
}
