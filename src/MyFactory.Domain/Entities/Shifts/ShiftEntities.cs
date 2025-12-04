using System;
using System.Collections.Generic;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Operations;
using MyFactory.Domain.Entities.Workshops;

namespace MyFactory.Domain.Entities.Shifts;

public sealed class ShiftPlan : BaseEntity
{
    private readonly List<ShiftPlanTask> _tasks = new();

    private ShiftPlan()
    {
    }

    public ShiftPlan(Guid employeeId, DateOnly shiftDate, string shiftCode)
    {
        Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));
        Guard.AgainstNullOrWhiteSpace(shiftCode, nameof(shiftCode));

        EmployeeId = employeeId;
        ShiftDate = shiftDate;
        ShiftCode = shiftCode.Trim();
        Status = ShiftPlanStatus.Draft;
    }

    public Guid EmployeeId { get; }
    public string ShiftCode { get; private set; } = string.Empty;
    public DateOnly ShiftDate { get; private set; }
    public ShiftPlanStatus Status { get; private set; }
    public IReadOnlyCollection<ShiftPlanTask> Tasks => _tasks.AsReadOnly();

    public void Approve()
    {
        if (Status != ShiftPlanStatus.Draft)
        {
            throw new DomainException("Only draft plans can be approved.");
        }

        if (_tasks.Count == 0)
        {
            throw new DomainException("Cannot approve plan without tasks.");
        }

        Status = ShiftPlanStatus.Approved;
    }

    public void Cancel(string reason)
    {
        if (Status == ShiftPlanStatus.Cancelled)
        {
            throw new DomainException("Plan already cancelled.");
        }

        Guard.AgainstNullOrWhiteSpace(reason, nameof(reason));
        Status = ShiftPlanStatus.Cancelled;
        ShiftCode = $"{ShiftCode} (Cancelled: {reason.Trim()})";
    }

    public ShiftPlanTask AddTask(Guid operationId, Guid workstationId, decimal plannedQuantity, decimal plannedHours)
    {
        var task = new ShiftPlanTask(Id, operationId, workstationId, plannedQuantity, plannedHours);
        _tasks.Add(task);
        return task;
    }
}

public sealed class ShiftPlanTask : BaseEntity
{
    private ShiftPlanTask()
    {
    }

    public ShiftPlanTask(Guid shiftPlanId, Guid operationId, Guid workstationId, decimal plannedQuantity, decimal plannedHours)
    {
        Guard.AgainstEmptyGuid(shiftPlanId, nameof(shiftPlanId));
        Guard.AgainstEmptyGuid(operationId, nameof(operationId));
        Guard.AgainstEmptyGuid(workstationId, nameof(workstationId));
        Guard.AgainstNonPositive(plannedQuantity, nameof(plannedQuantity));
        Guard.AgainstNonPositive(plannedHours, nameof(plannedHours));

        ShiftPlanId = shiftPlanId;
        OperationId = operationId;
        WorkstationId = workstationId;
        PlannedQuantity = plannedQuantity;
        PlannedHours = plannedHours;
    }

    public Guid ShiftPlanId { get; }
    public ShiftPlan? ShiftPlan { get; private set; }
    public Guid OperationId { get; }
    public Operation? Operation { get; private set; }
    public Guid WorkstationId { get; }
    public Workstation? Workstation { get; private set; }
    public decimal PlannedQuantity { get; private set; }
    public decimal PlannedHours { get; private set; }

    public void UpdatePlan(decimal plannedQuantity, decimal plannedHours)
    {
        Guard.AgainstNonPositive(plannedQuantity, nameof(plannedQuantity));
        Guard.AgainstNonPositive(plannedHours, nameof(plannedHours));
        PlannedQuantity = plannedQuantity;
        PlannedHours = plannedHours;
    }
}

public sealed class ShiftResult : BaseEntity
{
    private readonly List<ShiftResultOutput> _outputs = new();

    private ShiftResult()
    {
    }

    public ShiftResult(Guid employeeId, DateOnly shiftDate)
    {
        Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));
        EmployeeId = employeeId;
        ShiftDate = shiftDate;
        Status = ShiftResultStatus.InProgress;
    }

    public Guid EmployeeId { get; }
    public Guid? ShiftPlanId { get; private set; }
    public ShiftPlan? ShiftPlan { get; private set; }
    public DateOnly ShiftDate { get; private set; }
    public ShiftResultStatus Status { get; private set; }
    public decimal WorkedHours { get; private set; }
    public IReadOnlyCollection<ShiftResultOutput> Outputs => _outputs.AsReadOnly();

    public void LinkPlan(Guid shiftPlanId)
    {
        Guard.AgainstEmptyGuid(shiftPlanId, nameof(shiftPlanId));
        ShiftPlanId = shiftPlanId;
    }

    public void Complete(decimal workedHours)
    {
        Guard.AgainstNonPositive(workedHours, nameof(workedHours));
        WorkedHours = workedHours;
        Status = ShiftResultStatus.Completed;
    }

    public void Cancel()
    {
        if (Status == ShiftResultStatus.Completed)
        {
            throw new DomainException("Completed shift cannot be cancelled.");
        }

        Status = ShiftResultStatus.Cancelled;
    }

    public ShiftResultOutput AddOutput(Guid operationId, decimal quantity, string unit)
    {
        var output = new ShiftResultOutput(Id, operationId, quantity, unit);
        _outputs.Add(output);
        return output;
    }
}

public sealed class ShiftResultOutput : BaseEntity
{
    private ShiftResultOutput()
    {
    }

    public ShiftResultOutput(Guid shiftResultId, Guid operationId, decimal quantity, string unit)
    {
        Guard.AgainstEmptyGuid(shiftResultId, nameof(shiftResultId));
        Guard.AgainstEmptyGuid(operationId, nameof(operationId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Guard.AgainstNullOrWhiteSpace(unit, nameof(unit));

        ShiftResultId = shiftResultId;
        OperationId = operationId;
        Quantity = quantity;
        Unit = unit.Trim();
    }

    public Guid ShiftResultId { get; }
    public ShiftResult? ShiftResult { get; private set; }
    public Guid OperationId { get; }
    public Operation? Operation { get; private set; }
    public decimal Quantity { get; private set; }
    public string Unit { get; private set; } = string.Empty;
}

public enum ShiftPlanStatus
{
    Draft = 1,
    Approved = 2,
    Cancelled = 3
}

public enum ShiftResultStatus
{
    InProgress = 1,
    Completed = 2,
    Cancelled = 3
}
