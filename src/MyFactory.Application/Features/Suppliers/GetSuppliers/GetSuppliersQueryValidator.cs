using FluentValidation;

namespace MyFactory.Application.Features.Suppliers.GetSuppliers;

public sealed class GetSuppliersQueryValidator : AbstractValidator<GetSuppliersQuery>
{
    public GetSuppliersQueryValidator()
    {
        RuleFor(x => x.Search)
            .MaximumLength(200);
    }
}
