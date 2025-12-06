using FluentValidation;

namespace MyFactory.Application.Features.Warehousing.Commands.CreateWarehouse;

public sealed class CreateWarehouseCommandValidator : AbstractValidator<CreateWarehouseCommand>
{
    public CreateWarehouseCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(command => command.Type)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(command => command.Location)
            .NotEmpty()
            .MaximumLength(256);
    }
}
