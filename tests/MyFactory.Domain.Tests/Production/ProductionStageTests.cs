using System;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Domain.Tests.Production;

public class ProductionStageTests
{
    [Fact]
    public void Complete_Succeeds_AfterStageStarted()
    {
        var stage = ProductionStage.Create(Guid.NewGuid(), Guid.NewGuid(), "Assembly");
        var startedAt = DateTime.UtcNow;

        stage.Start(8m, startedAt);
        stage.Complete(8m, startedAt.AddHours(2));

        Assert.Equal(ProductionStageStatus.Completed, stage.Status);
    }

    [Fact]
    public void Complete_Throws_WhenStageNotStarted()
    {
        var stage = ProductionStage.Create(Guid.NewGuid(), Guid.NewGuid(), "Welding");

        Assert.Throws<DomainException>(() => stage.Complete(4m, DateTime.UtcNow));
    }
}
