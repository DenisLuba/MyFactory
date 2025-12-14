using System;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Domain.Tests.Production;

public class ProductionOrderTests
{
    [Fact]
    public void Complete_Succeeds_WhenAllStagesProduceOrderedQuantity()
    {
        var order = ProductionOrder.Create("PO-001", Guid.NewGuid(), 10m, DateTime.UtcNow);
        var stage = order.ScheduleStage(Guid.NewGuid(), "Assembly");

        order.Start();
        stage.Start(10m, DateTime.UtcNow);
        stage.Complete(10m, DateTime.UtcNow.AddHours(1));

        order.Complete();

        Assert.Equal(ProductionOrderStatus.Completed, order.Status);
    }

    [Fact]
    public void Start_WithoutScheduledStages_Throws()
    {
        var order = ProductionOrder.Create("PO-002", Guid.NewGuid(), 5m, DateTime.UtcNow);

        Assert.Throws<DomainException>(() => order.Start());
        Assert.Equal(ProductionOrderStatus.Planned, order.Status);
    }
}
