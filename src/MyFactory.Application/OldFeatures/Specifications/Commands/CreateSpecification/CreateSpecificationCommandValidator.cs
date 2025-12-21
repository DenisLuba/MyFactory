using FluentValidation;

namespace MyFactory.Application.OldFeatures.Specifications.Commands.CreateSpecification;

public sealed class CreateSpecificationCommandValidator : AbstractValidator<CreateSpecificationCommand>
{
    public CreateSpecificationCommandValidator()
    {
        RuleFor(command => command.Sku)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(command => command.PlanPerHour)
            .GreaterThan(0);

        RuleFor(command => command.Description)
            .MaximumLength(1024)
            .When(command => command.Description is not null);
    }
}
