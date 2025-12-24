using FluentValidation;

namespace MyFactory.Application.Features.Products.SetProductProductionCosts;

public sealed class SetProductProductionCostsCommandValidator
    : AbstractValidator<SetProductProductionCostsCommand>
{
    public SetProductProductionCostsCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.DepartmentId)
            .NotEmpty();

        RuleFor(x => x.ExpensesPerUnit)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.CutCostPerUnit)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.SewingCostPerUnit)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.PackCostPerUnit)
            .GreaterThanOrEqualTo(0);
    }
}