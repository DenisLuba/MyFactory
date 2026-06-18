using FluentValidation;

namespace MyFactory.Application.Features.ExpenseTypes.GetExpenseTypes;

public sealed class GetExpenseTypesQueryValidator : AbstractValidator<GetExpenseTypesQuery>
{
    public GetExpenseTypesQueryValidator()
    {
        // No filters yet; class exists for pipeline consistency.
    }
}
