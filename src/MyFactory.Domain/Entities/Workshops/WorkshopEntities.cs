using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.ValueObjects;

namespace MyFactory.Domain.Entities.Workshops;

public sealed class Workshop : BaseEntity
{
    private readonly List<WorkshopExpenseHistory> _expenseHistory = new();

    private Workshop()
    {
    }

    public Workshop(string name, string type)
    {
        Rename(name);
        ChangeType(type);
        IsActive = true;
    }

    public string Name { get; private set; } = string.Empty;

    public string Type { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public IReadOnlyCollection<WorkshopExpenseHistory> ExpenseHistory => _expenseHistory.AsReadOnly();

    public void Rename(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Workshop name is required.");
        Name = name.Trim();
    }

    public void ChangeType(string type)
    {
        Guard.AgainstNullOrWhiteSpace(type, "Workshop type is required.");
        Type = type.Trim();
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new DomainException("Workshop already inactive.");
        }

        IsActive = false;
    }

    public WorkshopExpenseHistory AddExpense(Guid specificationId, decimal amountPerUnit, DateOnly effectiveFrom, DateOnly? effectiveTo = null)
    {
        Guard.AgainstEmptyGuid(specificationId, "Specification id is required.");
        Guard.AgainstNonPositive(amountPerUnit, "Amount per unit must be positive.");
        Guard.AgainstDefaultDate(effectiveFrom, "Effective from date is required.");

        var range = DateRange.From(effectiveFrom, effectiveTo);

        var newEnd = range.End ?? DateOnly.MaxValue;

        var overlaps = _expenseHistory.Any(entry => entry.Overlaps(effectiveFrom, newEnd));
        if (overlaps)
        {
            throw new DomainException("Expense period overlaps with an existing record.");
        }

        var entry = new WorkshopExpenseHistory(Id, specificationId, amountPerUnit, effectiveFrom, effectiveTo);
        _expenseHistory.Add(entry);
        return entry;
    }
}

public sealed class WorkshopExpenseHistory : BaseEntity
{
    private WorkshopExpenseHistory()
    {
    }

    public WorkshopExpenseHistory(Guid workshopId, Guid specificationId, decimal amountPerUnit, DateOnly effectiveFrom, DateOnly? effectiveTo = null)
    {
        Guard.AgainstEmptyGuid(workshopId, "Workshop id is required.");
        Guard.AgainstEmptyGuid(specificationId, "Specification id is required.");
        Guard.AgainstNonPositive(amountPerUnit, "Amount per unit must be positive.");

        var range = DateRange.From(effectiveFrom, effectiveTo);

        WorkshopId = workshopId;
        SpecificationId = specificationId;
        AmountPerUnit = amountPerUnit;
        EffectiveFrom = range.Start;
        EffectiveTo = effectiveTo;
    }

    public Guid WorkshopId { get; private set; }

    public Workshop? Workshop { get; private set; }

    public Guid SpecificationId { get; private set; }

    public Specification? Specification { get; private set; }

    public decimal AmountPerUnit { get; private set; }

    public DateOnly EffectiveFrom { get; private set; }

    public DateOnly? EffectiveTo { get; private set; }

    public void UpdateAmount(decimal amountPerUnit)
    {
        Guard.AgainstNonPositive(amountPerUnit, "Amount per unit must be positive.");
        AmountPerUnit = amountPerUnit;
    }

    public void ClosePeriod(DateOnly effectiveTo)
    {
        Guard.AgainstDefaultDate(effectiveTo, "Effective to date is required.");
        DateRange.From(EffectiveFrom, effectiveTo);
        EffectiveTo = effectiveTo;
    }

    internal bool Overlaps(DateOnly start, DateOnly end)
    {
        var currentStart = EffectiveFrom;
        var currentEnd = EffectiveTo ?? DateOnly.MaxValue;
        return start <= currentEnd && currentStart <= end;
    }
}

