using FluentValidation;

namespace MyFactory.Application.Features.MaterialTypes.GetMaterialTypeDetails;

public sealed class GetMaterialTypeDetailsQueryValidator : AbstractValidator<GetMaterialTypeDetailsQuery>
{
    public GetMaterialTypeDetailsQueryValidator()
    {
        RuleFor(x => x.MaterialTypeId).NotEmpty();
    }
}
