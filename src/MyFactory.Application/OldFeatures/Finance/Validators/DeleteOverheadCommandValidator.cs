using FluentValidation;
using MyFactory.Application.Features.Finance.Commands.OverheadMonthly;

namespace MyFactory.Application.OldFeatures.Finance.Validators;

public sealed class DeleteOverheadCommandValidator : AbstractValidator<DeleteOverheadCommand>
{
    public DeleteOverheadCommandValidator()
    {
        RuleFor(command => command.OverheadMonthlyId).NotEmpty();
    }
}
