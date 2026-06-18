using FluentValidation;

namespace MyFactory.Application.Features.Materials.GetMaterialDetails;

public sealed class GetMaterialDetailsQueryValidator : AbstractValidator<GetMaterialDetailsQuery>
{
    public GetMaterialDetailsQueryValidator()
    {
        RuleFor(x => x.MaterialId)
            .NotEmpty();
    }
}
