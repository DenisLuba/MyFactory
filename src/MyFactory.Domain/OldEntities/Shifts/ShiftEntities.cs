using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Exceptions;
using MyFactory.Domain.OldEntities.Employees;
using MyFactory.Domain.OldEntities.Specifications;

namespace MyFactory.Domain.OldEntities.Shifts;

/// <summary>
/// Aggregate root describing a planned manufacturing shift for a specific employee and specification.
/// </summary>
public sealed class ShiftPlan : BaseEntity
{
    public const int ShiftTypeMaxLength = 50;

    private readonly List<ShiftResult> _results = new();

    private ShiftPlan()
    {
    }

    private ShiftPlan(Guid employeeId, Guid specificationId, DateOnly shiftDate, string shiftType, decimal plannedQuantity)
    {
        Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));
        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstDefaultDate(shiftDate, nameof(shiftDate));
        Guard.AgainstNullOrWhiteSpace(shiftType, nameof(shiftType));
        var stTrim = shiftType.Trim();
        if (stTrim.Length > ShiftTypeMaxLength)
        {
            throw new DomainException($"Shift type cannot exceed {ShiftTypeMaxLength} characters.");
        }
        Guard.AgainstNegative(plannedQuantity, nameof(plannedQuantity));

        EmployeeId = employeeId;
        SpecificationId = specificationId;
        ShiftDate = shiftDate;
        ShiftType = stTrim;
        PlannedQuantity = plannedQuantity;
    }

    public static ShiftPlan Create(Guid employeeId, Guid specificationId, DateOnly shiftDate, string shiftType, decimal plannedQuantity)
        => new ShiftPlan(employeeId, specificationId, shiftDate, shiftType, plannedQuantity);

    public Guid EmployeeId { get; private set; }
    public Employee? Employee { get; private set; }
    public Guid SpecificationId { get; private set; }
    public Specification? Specification { get; private set; }
    public DateOnly ShiftDate { get; private set; }
    public string ShiftType { get; private set; } = string.Empty;
    public decimal PlannedQuantity { get; private set; }

    public IReadOnlyCollection<ShiftResult> ShiftResults => _results.AsReadOnly();

    public void UpdatePlannedQuantity(decimal quantity)
    {
        Guard.AgainstNegative(quantity, nameof(quantity));
        PlannedQuantity = quantity;
    }

    public void UpdateShiftType(string shiftType)
    {
        Guard.AgainstNullOrWhiteSpace(shiftType, nameof(shiftType));
        var stTrim = shiftType.Trim();
        if (stTrim.Length > ShiftTypeMaxLength)
        {
            throw new DomainException($"Shift type cannot exceed {ShiftTypeMaxLength} characters.");
        }
        ShiftType = stTrim;
    }

    /// <summary>
    /// Create and attach a ShiftResult for this plan. Ensures employee consistency and sets navigation.
    /// </summary>
    public ShiftResult CreateResult(Guid employeeId, decimal actualQuantity, decimal hoursWorked, DateOnly recordedAt)
    {
        // Employee must match the plan's employee
        if (employeeId != EmployeeId)
        {
            throw new DomainException("Shift result employee must match the shift plan employee.");
        }

        var result = ShiftResult.Create(Id, employeeId, actualQuantity, hoursWorked, recordedAt);
        AttachResult(result);
        return result;
    }

    public void AddResult(ShiftResult result)
    {
        Guard.AgainstNull(result, nameof(result));
        if (result.ShiftPlanId != Id)
        {
            throw new DomainException("Shift result does not belong to this shift plan.");
        }
        if (result.EmployeeId != EmployeeId)
        {
            throw new DomainException("Shift result employee must match the shift plan employee.");
        }

        // Business allows multiple results per plan by ERD. If deduplication is required, enforce here (e.g., by RecordedAt).
        AttachResult(result);
    }

    internal void AttachResult(ShiftResult result)
    {
        Guard.AgainstNull(result, nameof(result));
        if (result.ShiftPlanId != Id)
        {
            throw new DomainException("Shift result does not belong to this shift plan.");
        }

        if (_results.Any(r => r.Id == result.Id))
        {
            return;
        }

        // keep in-memory graph consistent for domain logic and tests
        result.ShiftPlan = this;
        if (Employee != null)
        {
            result.Employee = Employee;
        }
        _results.Add(result);
    }

    internal void DetachResult(ShiftResult result)
    {
        Guard.AgainstNull(result, nameof(result));
        var index = _results.FindIndex(r => r.Id == result.Id);
        if (index == -1)
        {
            return;
        }
        _results.RemoveAt(index);
        result.ShiftPlan = null;
        result.Employee = null;
    }
}

/// <summary>
/// Aggregate root capturing actual results achieved during a shift.
/// </summary>
public sealed class ShiftResult : BaseEntity
{
    private ShiftResult()
    {
    }

    private ShiftResult(Guid shiftPlanId, Guid employeeId, decimal actualQuantity, decimal hoursWorked, DateOnly recordedAt)
    {
        Guard.AgainstEmptyGuid(shiftPlanId, nameof(shiftPlanId));
        Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));
        Guard.AgainstNegative(actualQuantity, nameof(actualQuantity));
        Guard.AgainstNegative(hoursWorked, nameof(hoursWorked));
        Guard.AgainstDefaultDate(recordedAt, nameof(recordedAt));

        ShiftPlanId = shiftPlanId;
        EmployeeId = employeeId;
        ActualQuantity = actualQuantity;
        HoursWorked = hoursWorked;
        RecordedAt = recordedAt;
    }

    public static ShiftResult Create(Guid shiftPlanId, Guid employeeId, decimal actualQuantity, decimal hoursWorked, DateOnly recordedAt)
        => new ShiftResult(shiftPlanId, employeeId, actualQuantity, hoursWorked, recordedAt);

    public Guid ShiftPlanId { get; private set; }
    public ShiftPlan? ShiftPlan { get; internal set; }
    public Guid EmployeeId { get; private set; }
    public Employee? Employee { get; internal set; }
    public decimal ActualQuantity { get; private set; }
    public decimal HoursWorked { get; private set; }
    public DateOnly RecordedAt { get; private set; }

    public void UpdateActualQuantity(decimal quantity)
    {
        Guard.AgainstNegative(quantity, nameof(quantity));
        ActualQuantity = quantity;
    }

    public void UpdateHoursWorked(decimal hours)
    {
        Guard.AgainstNegative(hours, nameof(hours));
        HoursWorked = hours;
    }

    public void UpdateRecordedAt(DateOnly recordedAt)
    {
        Guard.AgainstDefaultDate(recordedAt, nameof(recordedAt));
        RecordedAt = recordedAt;
    }
}

// TODO: Consider enforcing uniqueness of ShiftPlan per (EmployeeId, SpecificationId, ShiftDate, ShiftType) at repository level.
// TODO: Consider deduplication rules for ShiftResult (e.g., one result per employee per RecordedAt) if business requires.
// Testing notes:
// - Unit tests should cover invalid creation, mismatched employee when creating/adding results, attach/detach behavior, and multiple results scenarios.


