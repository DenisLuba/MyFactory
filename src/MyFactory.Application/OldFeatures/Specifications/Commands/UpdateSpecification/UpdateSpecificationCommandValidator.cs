using FluentValidation;

namespace MyFactory.Application.OldFeatures.Specifications.Commands.UpdateSpecification;

public sealed class UpdateSpecificationCommandValidator : AbstractValidator<UpdateSpecificationCommand>
{
    public UpdateSpecificationCommandValidator()
    {
        RuleFor(command => command.SpecificationId)
            .NotEmpty();

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
