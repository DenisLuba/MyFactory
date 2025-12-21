using FluentValidation;

namespace MyFactory.Application.Features.Materials.GetMaterials;

public sealed class GetMaterialsQueryValidator : AbstractValidator<GetMaterialsQuery>
{
    public GetMaterialsQueryValidator()
    {
        When(x => x.Filter is not null, () =>
        {
            RuleFor(x => x.Filter!.MaterialName)
                .MaximumLength(100);
            RuleFor(x => x.Filter!.MaterialType)
                .MaximumLength(100);
        });
    }
}
