using System;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Domain.Tests.Production;

public class WorkerAssignmentTests
{
    [Fact]
    public void CompleteWork_Succeeds_WhenWithinAssignedQuantity()
    {
        var stage = ProductionStage.Create(Guid.NewGuid(), Guid.NewGuid(), "Painting");
        stage.Start(10m, DateTime.UtcNow);

        var assignment = stage.AssignWorker(Guid.NewGuid(), 6m, DateTime.UtcNow);
        assignment.StartWork();
        assignment.CompleteWork(6m);

        Assert.Equal(WorkerAssignmentStatus.Completed, assignment.Status);
        Assert.Equal(6m, assignment.QuantityCompleted);
    }

    [Fact]
    public void AssignWorker_Throws_WhenCapacityExceeded()
    {
        var stage = ProductionStage.Create(Guid.NewGuid(), Guid.NewGuid(), "Finishing");
        stage.Start(10m, DateTime.UtcNow);
        stage.AssignWorker(Guid.NewGuid(), 8m, DateTime.UtcNow);

        Assert.Throws<DomainException>(() => stage.AssignWorker(Guid.NewGuid(), 5m, DateTime.UtcNow));
    }
}
