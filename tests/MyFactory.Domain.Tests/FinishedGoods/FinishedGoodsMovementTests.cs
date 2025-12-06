using System;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.FinishedGoods;
using Xunit;

namespace MyFactory.Domain.Tests.FinishedGoods;

public class FinishedGoodsMovementTests
{
    [Fact]
    public void CreateTransfer_WithValidData_Succeeds()
    {
        var movement = FinishedGoodsMovement.CreateTransfer(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            5,
            new DateTime(2025, 1, 5));

        Assert.Equal(5, movement.Quantity);
        Assert.Equal(new DateTime(2025, 1, 5), movement.MovedAt);
    }

    [Fact]
    public void CreateTransfer_WithSameWarehouses_Throws()
    {
        var warehouseId = Guid.NewGuid();

        Assert.Throws<DomainException>(() => FinishedGoodsMovement.CreateTransfer(
            Guid.NewGuid(),
            warehouseId,
            warehouseId,
            1,
            new DateTime(2025, 1, 5)));
    }
}
