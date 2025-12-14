using System;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Domain.Tests.Production;

public class ProductionOrderAllocationTests
{
    [Fact]
    public void AllocateWorkshop_Succeeds_WhenWithinOrderQuantity()
    {
        var order = ProductionOrder.Create("PO-100", Guid.NewGuid(), 100m, DateTime.UtcNow);

        var allocation = order.AllocateWorkshop(Guid.NewGuid(), 60m);

        Assert.Equal(60m, allocation.QuantityAllocated);
    }

    [Fact]
    public void AllocateWorkshop_Throws_WhenExceedingOrderQuantity()
    {
        var order = ProductionOrder.Create("PO-101", Guid.NewGuid(), 50m, DateTime.UtcNow);
        order.AllocateWorkshop(Guid.NewGuid(), 40m);

        Assert.Throws<DomainException>(() => order.AllocateWorkshop(Guid.NewGuid(), 20m));
    }
}
