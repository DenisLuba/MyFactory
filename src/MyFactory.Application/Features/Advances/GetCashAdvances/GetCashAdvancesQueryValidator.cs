using FluentValidation;

namespace MyFactory.Application.Features.Advances.GetCashAdvances;

public sealed class GetCashAdvancesQueryValidator : AbstractValidator<GetCashAdvancesQuery>
{
    public GetCashAdvancesQueryValidator()
    {
        RuleFor(x => x.From)
            .Must(d => d == null || d != default)
            .WithMessage("From date is invalid.");

        RuleFor(x => x.To)
            .Must(d => d == null || d != default)
            .WithMessage("To date is invalid.");

        RuleFor(x => x)
            .Must(x => !x.From.HasValue || !x.To.HasValue || x.To.Value >= x.From.Value)
            .WithMessage("To date must be greater than or equal to From date.");
    }
}
