using FluentValidation;
using MyFactory.Application.Features.Finance.Commands.OverheadMonthly;

namespace MyFactory.Application.Features.Finance.Validators;

public sealed class DeleteOverheadCommandValidator : AbstractValidator<DeleteOverheadCommand>
{
    public DeleteOverheadCommandValidator()
    {
        RuleFor(command => command.OverheadMonthlyId).NotEmpty();
    }
}
