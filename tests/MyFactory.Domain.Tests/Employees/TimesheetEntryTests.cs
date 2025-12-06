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
        var entry = new TimesheetEntry(employeeId, workDate, 8m, "Approved");

        Assert.Equal(employeeId, entry.EmployeeId);
        Assert.Equal(workDate, entry.WorkDate);
        Assert.Equal(8m, entry.Hours);
        Assert.Equal("Approved", entry.Status);
    }

    [Fact]
    public void Constructor_NegativeHours_Throws()
    {
        var employeeId = Guid.NewGuid();
        var workDate = new DateOnly(2025, 3, 15);

        Assert.Throws<DomainException>(() => new TimesheetEntry(employeeId, workDate, -1m, "Draft"));
    }
}
