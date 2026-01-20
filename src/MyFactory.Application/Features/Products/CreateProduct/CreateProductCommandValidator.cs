using FluentValidation;

namespace MyFactory.Application.Features.Products.CreateProduct;

public sealed class CreateProductCommandValidator
    : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.PlanPerHour)
            .GreaterThan(0)
            .When(x => x.PlanPerHour.HasValue);
    }
}

