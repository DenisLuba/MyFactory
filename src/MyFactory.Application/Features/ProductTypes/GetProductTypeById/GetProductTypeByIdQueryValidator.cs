using FluentValidation;

namespace MyFactory.Application.Features.ProductTypes.GetProductTypeById;

public sealed class GetProductTypeByIdQueryValidator : AbstractValidator<GetProductTypeByIdQuery>
{
    public GetProductTypeByIdQueryValidator()
    {
        RuleFor(x => x.ProductTypeId)
            .NotEmpty();
    }
}
