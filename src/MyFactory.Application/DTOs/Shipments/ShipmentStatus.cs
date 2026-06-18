namespace MyFactory.Application.DTOs.Shipments;

public enum ShipmentStatus
{
    Draft,
    Confirmed,
    Shipped,
    Cancelled
}

public static class ShipmentStatusExtensions
{
    public static ShipmentStatus? DomainToShipmentStatus(this Domain.Entities.Orders.ShipmentStatus? status) =>
        status switch
        {
            Domain.Entities.Orders.ShipmentStatus.Draft => ShipmentStatus.Draft,
            Domain.Entities.Orders.ShipmentStatus.Shipped => ShipmentStatus.Shipped,
            Domain.Entities.Orders.ShipmentStatus.Confirmed => ShipmentStatus.Confirmed,
            Domain.Entities.Orders.ShipmentStatus.Cancelled => ShipmentStatus.Cancelled,
            null => null,
            _ => throw new ArgumentOutOfRangeException(nameof(status), $"Not expected shipment status value: {status}"),
        };

    public static Domain.Entities.Orders.ShipmentStatus? ShipmentToDomainStatus(this ShipmentStatus? status) =>
        status switch
        {
            ShipmentStatus.Draft => Domain.Entities.Orders.ShipmentStatus.Draft,
            ShipmentStatus.Shipped => Domain.Entities.Orders.ShipmentStatus.Shipped,
            ShipmentStatus.Confirmed => Domain.Entities.Orders.ShipmentStatus.Confirmed,
            ShipmentStatus.Cancelled => Domain.Entities.Orders.ShipmentStatus.Cancelled,
            null => null,
            _ => throw new ArgumentOutOfRangeException(nameof(status), $"Not expected shipment status value: {status}"),
        };
}
