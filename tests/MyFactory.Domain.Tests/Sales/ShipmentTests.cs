using System;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Sales;
using MyFactory.Domain.Enums;
using Xunit;

namespace MyFactory.Domain.Tests.Sales;

public class ShipmentTests
{
    [Fact]
    public void Ship_Succeeds()
    {
        var shipment = new Shipment("SH-001", Guid.NewGuid(), new DateOnly(2025, 2, 1));
        shipment.AddItem(Guid.NewGuid(), 5, 100);

        shipment.Ship();
        shipment.MarkAsDelivered();

        Assert.Equal(ShipmentStatus.Delivered, shipment.Status);
        Assert.Equal(500, shipment.TotalAmount);
    }

    [Fact]
    public void ShipWithoutItems_Throws()
    {
        var shipment = new Shipment("SH-002", Guid.NewGuid(), new DateOnly(2025, 2, 1));

        Assert.Throws<DomainException>(() => shipment.Ship());
    }
}
