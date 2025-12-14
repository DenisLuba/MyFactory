using System;
using MyFactory.Domain.Entities.Shifts;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Domain.Tests.Shifts;

public class ShiftPlanTests
{
    [Fact]
    public void Constructor_WithValidData_SetsValues()
    {
        var employeeId = Guid.NewGuid();
        var specificationId = Guid.NewGuid();
        var shiftDate = new DateOnly(2025, 1, 5);

        var plan = new ShiftPlan(employeeId, specificationId, shiftDate, "Day", 120);

        Assert.Equal(employeeId, plan.EmployeeId);
        Assert.Equal(specificationId, plan.SpecificationId);
        Assert.Equal(shiftDate, plan.ShiftDate);
        Assert.Equal("Day", plan.ShiftType);
        Assert.Equal(120, plan.PlannedQuantity);
    }

    [Fact]
    public void Constructor_WithNegativeQty_Throws()
    {
        var employeeId = Guid.NewGuid();
        var specificationId = Guid.NewGuid();
        var shiftDate = new DateOnly(2025, 1, 5);

        Assert.Throws<DomainException>(() => new ShiftPlan(employeeId, specificationId, shiftDate, "Night", -1));
    }
}
