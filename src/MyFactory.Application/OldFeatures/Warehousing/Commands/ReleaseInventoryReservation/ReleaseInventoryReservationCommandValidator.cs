using FluentValidation;

namespace MyFactory.Application.OldFeatures.Warehousing.Commands.ReleaseInventoryReservation;

public sealed class ReleaseInventoryReservationCommandValidator : AbstractValidator<ReleaseInventoryReservationCommand>
{
    public ReleaseInventoryReservationCommandValidator()
    {
        RuleFor(command => command.WarehouseId)
            .NotEmpty();

        RuleFor(command => command.MaterialId)
            .NotEmpty();

        RuleFor(command => command.Quantity)
            .GreaterThan(0m);
    }
}
