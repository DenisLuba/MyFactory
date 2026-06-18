using FluentValidation;

namespace MyFactory.Application.Features.MaterialTypes.GetMaterialTypes;

public sealed class GetMaterialTypesQueryValidator : AbstractValidator<GetMaterialTypesQuery>
{
    public GetMaterialTypesQueryValidator()
    {
        // No fields to validate; class provided for consistency and future extension.
    }
}
