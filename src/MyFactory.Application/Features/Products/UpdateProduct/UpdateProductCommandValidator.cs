using FluentValidation;

namespace MyFactory.Application.Features.Products.UpdateProduct;

public sealed class UpdateProductCommandValidator
    : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.PlanPerHour)
            .GreaterThan(0)
            .When(x => x.PlanPerHour.HasValue);
    }
}