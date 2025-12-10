using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Specifications;

namespace MyFactory.Domain.Entities.Workshops;

public sealed class Workshop : BaseEntity
{
    public const int NameMaxLength = 100;
    public const int TypeMaxLength = 100;

    private readonly List<WorkshopExpenseHistory> _expenseHistory = new();

    private Workshop()
    {
    }

    private Workshop(string name, string type)
    {
        Rename(name);
        ChangeType(type);
        IsActive = true;
    }

    public static Workshop Create(string name, string type) => new(name, type);

    public string Name { get; private set; } = string.Empty;

    public string Type { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public IReadOnlyCollection<WorkshopExpenseHistory> ExpenseHistory => _expenseHistory.AsReadOnly();

    public void Rename(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Workshop name is required.");
        var trimmed = name.Trim();
        if (trimmed.Length > NameMaxLength)
        {
            throw new DomainException($"Workshop name cannot exceed {NameMaxLength} characters.");
        }

        Name = trimmed;
    }

    public void ChangeType(string type)
    {
        Guard.AgainstNullOrWhiteSpace(type, "Workshop type is required.");
        var trimmed = type.Trim();
        if (trimmed.Length > TypeMaxLength)
        {
            throw new DomainException($"Workshop type cannot exceed {TypeMaxLength} characters.");
        }

        Type = trimmed;
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new DomainException("Workshop already inactive.");
        }

        IsActive = false;
    }

    public void Activate()
    {
        if (IsActive)
        {
            throw new DomainException("Workshop already active.");
        }

        IsActive = true;
    }

    public WorkshopExpenseHistory AddExpense(Guid specificationId, decimal amountPerUnit, DateOnly effectiveFrom, DateOnly? effectiveTo = null)
    {
        Guard.AgainstEmptyGuid(specificationId, "Specification id is required.");
        Guard.AgainstNonPositive(amountPerUnit, "Amount per unit must be positive.");
        Guard.AgainstDefaultDate(effectiveFrom, "Effective from date is required.");
        if (effectiveTo != null && effectiveTo.Value < effectiveFrom)
        {
            throw new DomainException("Effective to date cannot be earlier than effective from date.");
        }

        var normalizedNewEnd = effectiveTo ?? DateOnly.MaxValue;

        var overlappingEntry = _expenseHistory.Any(entry => entry.SpecificationId == specificationId && entry.Overlaps(effectiveFrom, normalizedNewEnd));
        if (overlappingEntry)
        {
            throw new DomainException("Expense period overlaps with an existing record.");
        }

        var openEntry = _expenseHistory
            .Where(entry => entry.SpecificationId == specificationId && entry.EffectiveTo == null)
            .OrderByDescending(entry => entry.EffectiveFrom)
            .FirstOrDefault();
        if (openEntry != null)
        {
            if (effectiveFrom <= openEntry.EffectiveFrom)
            {
                throw new DomainException("New expense effective_from must be after the previous open period start.");
            }

            openEntry.ClosePeriod(effectiveFrom.AddDays(-1));
        }

        var expense = WorkshopExpenseHistory.Create(Id, specificationId, amountPerUnit, effectiveFrom, effectiveTo);
        _expenseHistory.Add(expense);
        return expense;
    }
}

public sealed class WorkshopExpenseHistory : BaseEntity
{
    private WorkshopExpenseHistory()
    {
    }

    private WorkshopExpenseHistory(Guid workshopId, Guid specificationId, decimal amountPerUnit, DateOnly effectiveFrom, DateOnly? effectiveTo)
    {
        Guard.AgainstEmptyGuid(workshopId, "Workshop id is required.");
        Guard.AgainstEmptyGuid(specificationId, "Specification id is required.");
        Guard.AgainstNonPositive(amountPerUnit, "Amount per unit must be positive.");
        Guard.AgainstDefaultDate(effectiveFrom, "Effective from date is required.");
        if (effectiveTo != null && effectiveTo.Value < effectiveFrom)
        {
            throw new DomainException("Effective to date cannot be earlier than effective from date.");
        }

        WorkshopId = workshopId;
        SpecificationId = specificationId;
        AmountPerUnit = amountPerUnit;
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
    }

    public static WorkshopExpenseHistory Create(Guid workshopId, Guid specificationId, decimal amountPerUnit, DateOnly effectiveFrom, DateOnly? effectiveTo)
        => new(workshopId, specificationId, amountPerUnit, effectiveFrom, effectiveTo);

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
        if (effectiveTo < EffectiveFrom)
        {
            throw new DomainException("Effective to date cannot be earlier than effective from date.");
        }

        EffectiveTo = effectiveTo;
    }

    internal bool Overlaps(DateOnly periodStart, DateOnly periodEnd)
    {
        var currentStart = EffectiveFrom;
        var currentEnd = EffectiveTo ?? DateOnly.MaxValue;
        return periodStart <= currentEnd && currentStart <= periodEnd;
    }
}

