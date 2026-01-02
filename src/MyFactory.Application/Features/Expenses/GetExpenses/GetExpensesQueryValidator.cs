using FluentValidation;

namespace MyFactory.Application.Features.Expenses.GetExpenses;

public sealed class GetExpensesQueryValidator : AbstractValidator<GetExpensesQuery>
{
    public GetExpensesQueryValidator()
    {
        RuleFor(x => x.From)
            .Must(d => d != default)
            .WithMessage("From date is required.");

        RuleFor(x => x.To)
            .Must(d => d != default)
            .WithMessage("To date is required.");

        RuleFor(x => x)
            .Must(q => q.To >= q.From)
            .WithMessage("To date must be greater than or equal to From date.");
    }
}
