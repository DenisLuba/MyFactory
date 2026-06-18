using FluentValidation;

namespace MyFactory.Application.Features.Products.SetProductProductionCosts;

public sealed class SetProductProductionCostsCommandValidator
    : AbstractValidator<SetProductProductionCostsCommand>
{
    public SetProductProductionCostsCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();
    }
}