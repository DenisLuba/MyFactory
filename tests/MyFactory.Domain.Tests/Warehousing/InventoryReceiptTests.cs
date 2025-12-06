using System;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Warehousing;
using Xunit;

namespace MyFactory.Domain.Tests.Warehousing;

public class InventoryReceiptTests
{
    [Fact]
    public void AddItemAndMarkAsReceived_UpdatesTotalAndStatus()
    {
        var receipt = new InventoryReceipt("RC-1", Guid.NewGuid(), new DateTime(2025, 1, 1));

        receipt.AddItem(Guid.NewGuid(), 5, 10);
        receipt.AddItem(Guid.NewGuid(), 3, 4);
        receipt.MarkAsReceived();

        Assert.Equal(InventoryReceiptStatus.Received, receipt.Status);
        Assert.Equal(5 * 10 + 3 * 4, receipt.TotalAmount);
    }

    [Fact]
    public void MarkAsReceived_WithoutItems_Throws()
    {
        var receipt = new InventoryReceipt("RC-2", Guid.NewGuid(), new DateTime(2025, 1, 1));

        Assert.Throws<DomainException>(() => receipt.MarkAsReceived());
    }
}
