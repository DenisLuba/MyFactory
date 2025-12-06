using System;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Sales;
using Xunit;

namespace MyFactory.Domain.Tests.Sales;

public class ShipmentTests
{
    [Fact]
    public void SubmitAndShip_Succeeds()
    {
        var shipment = new Shipment("SH-001", Guid.NewGuid(), new DateTime(2025, 2, 1));
        shipment.AddItem(Guid.NewGuid(), 5, 100);

        shipment.Submit();
        shipment.MarkAsShipped();

        Assert.Equal(ShipmentStatus.Shipped, shipment.Status);
        Assert.Equal(500, shipment.TotalAmount);
    }

    [Fact]
    public void SubmitWithoutItems_Throws()
    {
        var shipment = new Shipment("SH-002", Guid.NewGuid(), new DateTime(2025, 2, 1));

        Assert.Throws<DomainException>(() => shipment.Submit());
    }
}
