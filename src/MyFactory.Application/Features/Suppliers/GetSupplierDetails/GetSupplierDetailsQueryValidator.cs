using FluentValidation;

namespace MyFactory.Application.Features.Suppliers.GetSupplierDetails;

public sealed class GetSupplierDetailsQueryValidator
    : AbstractValidator<GetSupplierDetailsQuery>
{
    public GetSupplierDetailsQueryValidator()
    {
        RuleFor(x => x.SupplierId)
            .NotEmpty();
    }
}
