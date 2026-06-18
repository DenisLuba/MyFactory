using FluentValidation;

namespace MyFactory.Application.Features.Materials.GetMaterialImages;

public sealed class GetMaterialImagesQueryValidator : AbstractValidator<GetMaterialImagesQuery>
{
    public GetMaterialImagesQueryValidator()
    {
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
