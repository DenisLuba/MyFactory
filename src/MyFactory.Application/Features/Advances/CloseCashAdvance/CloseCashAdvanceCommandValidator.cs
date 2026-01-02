using FluentValidation;

namespace MyFactory.Application.Features.Advances.CloseCashAdvance;

public sealed class CloseCashAdvanceCommandValidator : AbstractValidator<CloseCashAdvanceCommand>
{
    public CloseCashAdvanceCommandValidator()
    {
        RuleFor(x => x.CashAdvanceId)
            .NotEmpty();
    }
}
