using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Specifications;

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

    public WorkshopExpenseHistory AddExpense(Guid specificationId, decimal amountPerUnit, DateTime effectiveFrom, DateTime? effectiveTo = null)
    {
        Guard.AgainstEmptyGuid(specificationId, "Specification id is required.");
        Guard.AgainstNonPositive(amountPerUnit, "Amount per unit must be positive.");
        Guard.AgainstDefaultDate(effectiveFrom, "Effective from date is required.");

        if (effectiveTo.HasValue && effectiveTo.Value < effectiveFrom)
        {
            throw new DomainException("Effective to date cannot be earlier than effective from date.");
        }

        var newEnd = effectiveTo ?? DateTime.MaxValue;

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

    public WorkshopExpenseHistory(Guid workshopId, Guid specificationId, decimal amountPerUnit, DateTime effectiveFrom, DateTime? effectiveTo = null)
    {
        Guard.AgainstEmptyGuid(workshopId, "Workshop id is required.");
        Guard.AgainstEmptyGuid(specificationId, "Specification id is required.");
        Guard.AgainstNonPositive(amountPerUnit, "Amount per unit must be positive.");
        Guard.AgainstDefaultDate(effectiveFrom, "Effective from date is required.");

        if (effectiveTo.HasValue && effectiveTo.Value < effectiveFrom)
        {
            throw new DomainException("Effective to date cannot be earlier than effective from date.");
        }

        WorkshopId = workshopId;
        SpecificationId = specificationId;
        AmountPerUnit = amountPerUnit;
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
    }

    public Guid WorkshopId { get; private set; }

    public Workshop? Workshop { get; private set; }

    public Guid SpecificationId { get; private set; }

    public Specification? Specification { get; private set; }

    public decimal AmountPerUnit { get; private set; }

    public DateTime EffectiveFrom { get; private set; }

    public DateTime? EffectiveTo { get; private set; }

    public void UpdateAmount(decimal amountPerUnit)
    {
        Guard.AgainstNonPositive(amountPerUnit, "Amount per unit must be positive.");
        AmountPerUnit = amountPerUnit;
    }

    public void ClosePeriod(DateTime effectiveTo)
    {
        Guard.AgainstDefaultDate(effectiveTo, "Effective to date is required.");

        if (effectiveTo < EffectiveFrom)
        {
            throw new DomainException("Effective to date cannot be earlier than effective from date.");
        }

        EffectiveTo = effectiveTo;
    }

    internal bool Overlaps(DateTime start, DateTime end)
    {
        var currentStart = EffectiveFrom;
        var currentEnd = EffectiveTo ?? DateTime.MaxValue;
        return start <= currentEnd && currentStart <= end;
    }
}

