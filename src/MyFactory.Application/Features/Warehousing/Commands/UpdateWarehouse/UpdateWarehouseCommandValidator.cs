using System;
using FluentValidation;

namespace MyFactory.Application.Features.Warehousing.Commands.UpdateWarehouse;

public sealed class UpdateWarehouseCommandValidator : AbstractValidator<UpdateWarehouseCommand>
{
    public UpdateWarehouseCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

        RuleFor(command => command)
            .Must(command => command.Name is not null || command.Type is not null || command.Location is not null)
            .WithMessage("At least one field must be provided for update.");

        When(command => command.Name is not null, () =>
        {
            RuleFor(command => command.Name!)
                .NotEmpty()
                .MaximumLength(256);
        });

        When(command => command.Type is not null, () =>
        {
            RuleFor(command => command.Type!)
                .NotEmpty()
                .MaximumLength(128);
        });

        When(command => command.Location is not null, () =>
        {
            RuleFor(command => command.Location!)
                .NotEmpty()
                .MaximumLength(256);
        });
    }
}
