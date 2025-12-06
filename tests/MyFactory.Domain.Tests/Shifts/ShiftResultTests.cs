using System;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Shifts;
using Xunit;

namespace MyFactory.Domain.Tests.Shifts;

public class ShiftResultTests
{
    [Fact]
    public void Constructor_WithValidData_SetsValues()
    {
        var employeeId = Guid.NewGuid();
        var specificationId = Guid.NewGuid();
        var shiftDate = new DateOnly(2025, 2, 10);
        var shiftPlan = new ShiftPlan(employeeId, specificationId, shiftDate, "Day", 120);
        var recordedAt = new DateTime(2025, 2, 10, 18, 30, 0, DateTimeKind.Utc);

        var result = new ShiftResult(shiftPlan.Id, 95, 7.5m, recordedAt);

        Assert.Equal(shiftPlan.Id, result.ShiftPlanId);
        Assert.Equal(95, result.ActualQuantity);
        Assert.Equal(7.5m, result.HoursWorked);
        Assert.Equal(recordedAt, result.RecordedAt);
    }

    [Fact]
    public void Constructor_WithNegativeActualQty_Throws()
    {
        var shiftPlan = new ShiftPlan(Guid.NewGuid(), Guid.NewGuid(), new DateOnly(2025, 2, 10), "Night", 150);

        Assert.Throws<DomainException>(() => new ShiftResult(shiftPlan.Id, -5, 4, DateTime.UtcNow));
    }
}
