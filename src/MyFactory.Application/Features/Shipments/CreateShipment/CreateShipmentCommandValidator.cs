using FluentValidation;
using MyFactory.Application.DTOs.Shipments;

namespace MyFactory.Application.Features.Shipments.CreateShipment;

public sealed class CreateShipmentCommandValidator : AbstractValidator<CreateShipmentCommand>
{
    public CreateShipmentCommandValidator()
    {
        RuleFor(x => x.SalesOrderId).NotEmpty();
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.CreatedBy).NotEmpty();
        RuleFor(x => x.ShipmentDate).Must(d => d != default).WithMessage("ShipmentDate is required");

        RuleFor(x => x.Status)
            .Must(s => s is null || Enum.IsDefined(typeof(ShipmentStatus), s))
            .WithMessage("Invalid shipment status");

        RuleFor(x => x.Items)
            .NotNull()
            .NotEmpty().WithMessage("Shipment must contain at least one item");

        RuleForEach(x => x.Items).SetValidator(new CreateShipmentItemDtoValidator());
    }
}

public sealed class CreateShipmentItemDtoValidator : AbstractValidator<CreateShipmentItemDto>
{
    public CreateShipmentItemDtoValidator()
    {
        RuleFor(i => i.SalesOrderItemId).NotEmpty();
        RuleFor(i => i.ProductId).NotEmpty();
        RuleFor(i => i.WarehouseId).NotEmpty();
        RuleFor(i => i.Qty).GreaterThan(0);
        RuleFor(i => i.UnitPrice).GreaterThanOrEqualTo(0);
    }
}
