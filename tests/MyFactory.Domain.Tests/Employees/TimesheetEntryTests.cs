using System;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Employees;
using Xunit;

namespace MyFactory.Domain.Tests.Employees;

public class TimesheetEntryTests
{
    [Fact]
    public void Constructor_Valid_SetsValues()
    {
        var employeeId = Guid.NewGuid();
        var workDate = new DateOnly(2025, 3, 15);
        var productionOrderId = Guid.NewGuid();
        var entry = TimesheetEntry.Create(employeeId, workDate, 8m, productionOrderId);

        Assert.Equal(employeeId, entry.EmployeeId);
        Assert.Equal(workDate, entry.WorkDate);
        Assert.Equal(8m, entry.HoursWorked);
        Assert.Equal(productionOrderId, entry.ProductionOrderId);
        Assert.Equal(TimesheetEntryStatus.Draft, entry.Status);
    }

    [Fact]
    public void Constructor_NegativeHours_Throws()
    {
        var employeeId = Guid.NewGuid();
        var workDate = new DateOnly(2025, 3, 15);

        Assert.Throws<DomainException>(() => TimesheetEntry.Create(employeeId, workDate, -1m, null));
    }

    [Fact]
    public void Approve_WhenDraft_SetsStatusAndLocksEditing()
    {
        var entry = TimesheetEntry.Create(Guid.NewGuid(), new DateOnly(2025, 3, 15), 8m, null);

        entry.Approve();

        Assert.Equal(TimesheetEntryStatus.Approved, entry.Status);
        Assert.Throws<DomainException>(() => entry.UpdateHours(9m));
    }

    [Fact]
    public void ReturnToDraft_FromApproved_AllowsChanges()
    {
        var entry = TimesheetEntry.Create(Guid.NewGuid(), new DateOnly(2025, 3, 15), 8m, null);
        entry.Approve();
        entry.ReturnToDraft();

        Assert.Equal(TimesheetEntryStatus.Draft, entry.Status);
        entry.UpdateHours(6m);

        Assert.Equal(6m, entry.HoursWorked);
    }
}
