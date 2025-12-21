using FluentValidation;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.OldFeatures.Advances.Commands.AddAdvanceReport;

public sealed class AddAdvanceReportCommandValidator : AbstractValidator<AddAdvanceReportCommand>
{
    public AddAdvanceReportCommandValidator()
    {
        RuleFor(cmd => cmd.AdvanceId).NotEmpty();
        RuleFor(cmd => cmd.Description)
            .NotEmpty()
            .MaximumLength(AdvanceReport.DescriptionMaxLength);
        RuleFor(cmd => cmd.Amount).GreaterThan(0);
        RuleFor(cmd => cmd.ReportedAt).Must(date => date != default)
            .WithMessage("Reported date is required.");
        RuleFor(cmd => cmd.FileId).NotEmpty();
        RuleFor(cmd => cmd.SpentAt).Must(date => date != default)
            .WithMessage("Spent date is required.");
        RuleFor(cmd => cmd)
            .Must(cmd => cmd.SpentAt <= cmd.ReportedAt)
            .WithMessage("Spent date cannot be later than reported date.");
    }
}
