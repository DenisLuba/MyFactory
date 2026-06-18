using FluentValidation;

namespace MyFactory.Application.Features.Materials.GetMaterialImage;

public sealed class GetMaterialImageQueryValidator : AbstractValidator<GetMaterialImageQuery>
{
    public GetMaterialImageQueryValidator()
    {
        RuleFor(x => x.ImageId).NotEmpty();
    }
}
