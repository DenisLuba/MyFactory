using System;
using MyFactory.Domain.Entities.Warehousing;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Domain.Tests.Warehousing;

public class WarehouseTests
{
    [Fact]
    public void AddInventoryAndReceiveStock_ComputesAveragePrice()
    {
        var warehouse = new Warehouse("Main", "Raw", "A1");
        var materialId = Guid.NewGuid();

        var inventory = warehouse.AddInventory(materialId);
        inventory.Receive(10, 5);
        inventory.Receive(10, 7);

        Assert.Equal(20, inventory.Quantity);
        Assert.Equal(6, inventory.AveragePrice);
    }

    [Fact]
    public void ReceiveStock_WithNonPositiveQuantity_Throws()
    {
        var warehouse = new Warehouse("Main", "Raw", "A1");
        var inventory = warehouse.AddInventory(Guid.NewGuid());

        Assert.Throws<DomainException>(() => inventory.Receive(0, 5));
    }
}
