using FluentValidation;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.Features.Advances.Commands.AddAdvanceReport;

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
    }
}
