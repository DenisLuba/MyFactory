using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Operations;
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

#region Legacy workstation entities (to be revisited in future modules)

public sealed class Workstation : BaseEntity
{
    public Workstation(Guid id, Guid workshopId, string code)
        : base(id)
    {
        Guard.AgainstEmptyGuid(workshopId, nameof(workshopId));
        Guard.AgainstNullOrWhiteSpace(code, nameof(code));

        WorkshopId = workshopId;
        Code = code;
    }

    public Guid WorkshopId { get; }
    public string Code { get; private set; }
    public string? Description { get; private set; }
    public Guid? CurrentEmployeeId { get; private set; }
    public Employee? CurrentEmployee { get; private set; }
    public ICollection<Machine> Machines { get; private set; } = new List<Machine>();
    public ICollection<WorkstationMaterialBuffer> Buffers { get; private set; } = new List<WorkstationMaterialBuffer>();

    public void AssignEmployee(Employee employee)
    {
        Guard.AgainstNull(employee, nameof(employee));

        CurrentEmployee = employee;
        CurrentEmployeeId = employee.Id;
    }

    public void UnassignEmployee()
    {
        CurrentEmployee = null;
        CurrentEmployeeId = null;
    }
}

public sealed class Machine : BaseEntity
{
    public Machine(Guid id, Guid workstationId, string serialNumber)
        : base(id)
    {
        Guard.AgainstEmptyGuid(workstationId, nameof(workstationId));
        Guard.AgainstNullOrWhiteSpace(serialNumber, nameof(serialNumber));

        WorkstationId = workstationId;
        SerialNumber = serialNumber;
    }

    public Guid WorkstationId { get; }
    public string SerialNumber { get; private set; }
    public string? Model { get; private set; }
    public Guid? OperationId { get; private set; }
    public Operation? Operation { get; private set; }

    public void AssignOperation(Operation operation)
    {
        Guard.AgainstNull(operation, nameof(operation));
        Operation = operation;
        OperationId = operation.Id;
    }
}

public sealed class WorkstationMaterialBuffer : BaseEntity
{
    public WorkstationMaterialBuffer(Guid id, Guid workstationId, Guid materialId, decimal capacity)
        : base(id)
    {
        Guard.AgainstEmptyGuid(workstationId, nameof(workstationId));
        Guard.AgainstEmptyGuid(materialId, nameof(materialId));
        Guard.AgainstNegative(capacity, nameof(capacity));

        WorkstationId = workstationId;
        MaterialId = materialId;
        Capacity = capacity;
    }

    public Guid WorkstationId { get; }
    public Guid MaterialId { get; }
    public decimal Capacity { get; private set; }
    public decimal CurrentLevel { get; private set; }

    public void SetLevel(decimal level)
    {
        Guard.AgainstNegative(level, nameof(level));
        if (level > Capacity)
        {
            throw new DomainException("Buffer level cannot exceed capacity.");
        }

        CurrentLevel = level;
    }
}

#endregion