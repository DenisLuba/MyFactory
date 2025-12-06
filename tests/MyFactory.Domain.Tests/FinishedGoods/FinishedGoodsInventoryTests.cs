using System;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.FinishedGoods;
using Xunit;

namespace MyFactory.Domain.Tests.FinishedGoods;

public class FinishedGoodsInventoryTests
{
    [Fact]
    public void ReceiveAndIssue_UpdatesQuantityAndCost()
    {
        var inventory = new FinishedGoodsInventory(Guid.NewGuid(), Guid.NewGuid());

        inventory.Receive(10, 5, new DateTime(2025, 1, 1));
        inventory.Receive(10, 7, new DateTime(2025, 1, 2));
        inventory.Issue(5, new DateTime(2025, 1, 3));

        Assert.Equal(15, inventory.Quantity);
        Assert.Equal(new DateTime(2025, 1, 3), inventory.UpdatedAt);
        Assert.Equal(6, inventory.UnitCost); // weighted average (10*5 + 10*7) / 20
    }

    [Fact]
    public void Issue_MoreThanOnHand_Throws()
    {
        var inventory = new FinishedGoodsInventory(Guid.NewGuid(), Guid.NewGuid());
        inventory.Receive(5, 5, new DateTime(2025, 1, 1));

        Assert.Throws<DomainException>(() => inventory.Issue(6, new DateTime(2025, 1, 2)));
    }
}
