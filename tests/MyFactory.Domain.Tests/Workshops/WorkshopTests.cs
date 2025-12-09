using System;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Workshops;
using Xunit;

namespace MyFactory.Domain.Tests.Workshops;

public class WorkshopTests
{
    [Fact]
    public void AddExpense_WithUniquePeriod_AddsEntry()
    {
        var workshop = new Workshop("Main sewing", "Sewing");
        var specificationId = Guid.NewGuid();
        var effectiveFrom = new DateOnly(2025, 1, 1);

        var expense = workshop.AddExpense(specificationId, 5.5m, effectiveFrom, effectiveFrom.AddDays(10));

        Assert.Single(workshop.ExpenseHistory);
        Assert.Equal(expense, workshop.ExpenseHistory.Single());
        Assert.Equal(5.5m, expense.AmountPerUnit);
        Assert.Equal(effectiveFrom, expense.EffectiveFrom);
    }

    [Fact]
    public void AddExpense_WithOverlappingPeriod_Throws()
    {
        var workshop = new Workshop("Assembly", "Assembly");
        var specificationId = Guid.NewGuid();
        workshop.AddExpense(specificationId, 4m, new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 31));

        Assert.Throws<DomainException>(() =>
            workshop.AddExpense(specificationId, 6m, new DateOnly(2025, 1, 15), new DateOnly(2025, 2, 15)));
    }

    [Fact]
    public void AddExpense_WithEmptySpecificationId_Throws()
    {
        var workshop = new Workshop("Finishing", "Finishing");

        Assert.Throws<DomainException>(() =>
            workshop.AddExpense(Guid.Empty, 3m, new DateOnly(2025, 3, 1), null));
    }
}
