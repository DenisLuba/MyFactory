using System;
using FluentValidation;

namespace MyFactory.Application.Features.Materials.Commands.AddMaterialPrice;

public sealed class AddMaterialPriceCommandValidator : AbstractValidator<AddMaterialPriceCommand>
{
    public AddMaterialPriceCommandValidator()
    {
        RuleFor(command => command.MaterialId)
            .NotEmpty();

        RuleFor(command => command.SupplierId)
            .NotEmpty();

        RuleFor(command => command.Price)
            .GreaterThan(0m);

        RuleFor(command => command.EffectiveFrom)
            .NotEmpty();

        RuleFor(command => command.DocRef)
            .NotEmpty()
            .MaximumLength(128);

        When(command => command.EffectiveTo.HasValue, () =>
        {
            RuleFor(command => command)
                .Must(command => command.EffectiveTo!.Value >= command.EffectiveFrom)
                .WithMessage("Effective to date must be greater than or equal to effective from date.");
        });
    }
}
