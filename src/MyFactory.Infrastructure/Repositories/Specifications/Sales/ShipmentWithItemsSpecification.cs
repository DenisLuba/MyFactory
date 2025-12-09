using System;
using MyFactory.Domain.Entities.Sales;

namespace MyFactory.Infrastructure.Repositories.Specifications;

public sealed class ShipmentWithItemsSpecification : Specification<Shipment>
{
    public ShipmentWithItemsSpecification(Guid shipmentId)
        : base(shipment => shipment.Id == shipmentId)
    {
        AddInclude(shipment => shipment.Customer!);
        AddInclude(shipment => shipment.Items);
        AddInclude($"{nameof(Shipment.Items)}.{nameof(ShipmentItem.Specification)}");
        AsNoTrackingQuery();
    }
}
