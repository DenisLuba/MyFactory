using System;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Domain.Tests.Employees;

public class PayrollEntryTests
{
    [Fact]
    public void RecalculateOutstanding_Valid_Works()
    {
        var entry = PayrollEntry.Create(
            Guid.NewGuid(),
            new DateOnly(2025, 1, 1),
            new DateOnly(2025, 1, 31),
            160m,
            1_000m);

        entry.AddPayment(250m);
        entry.RecalculateOutstanding();

        Assert.Equal(750m, entry.Outstanding);
        Assert.Equal(250m, entry.PaidAmount);
    }

    [Fact]
    public void AddPayment_ExceedingAccrued_Throws()
    {
        var entry = PayrollEntry.Create(
            Guid.NewGuid(),
            new DateOnly(2025, 2, 1),
            new DateOnly(2025, 2, 28),
            120m,
            800m);

        entry.AddPayment(500m);

        Assert.Throws<DomainException>(() => entry.AddPayment(400m));
    }
}
