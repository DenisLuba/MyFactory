using FluentValidation;

namespace MyFactory.Application.Features.ExpenseTypes.GetExpenseTypeDetails;

public sealed class GetExpenseTypeDetailsQueryValidator : AbstractValidator<GetExpenseTypeDetailsQuery>
{
    public GetExpenseTypeDetailsQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
