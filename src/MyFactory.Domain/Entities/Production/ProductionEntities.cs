using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.Entities.Warehousing;
using MyFactory.Domain.Entities.Workshops;

namespace MyFactory.Domain.Entities.Production;

public sealed class ProductionOrder : BaseEntity
{
    private readonly List<ProductionOrderAllocation> _allocations = new();
    private readonly List<ProductionStage> _stages = new();

    private ProductionOrder()
    {
    }

    private ProductionOrder(string orderNumber, Guid specificationId, decimal quantityOrdered, DateTime createdAt)
    {
        Guard.AgainstNullOrWhiteSpace(orderNumber, nameof(orderNumber));
        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstNonPositive(quantityOrdered, nameof(quantityOrdered));
        Guard.AgainstDefaultDate(createdAt, nameof(createdAt));

        OrderNumber = orderNumber.Trim();
        SpecificationId = specificationId;
        QuantityOrdered = quantityOrdered;
        CreatedAt = createdAt;
        Status = ProductionOrderStatuses.Planned;
    }

    public static ProductionOrder Create(string orderNumber, Guid specificationId, decimal quantityOrdered, DateTime createdAt)
    {
        return new ProductionOrder(orderNumber, specificationId, quantityOrdered, createdAt);
    }

    public string OrderNumber { get; private set; } = string.Empty;
    public Guid SpecificationId { get; private set; }
    public Specification? Specification { get; private set; }
    public decimal QuantityOrdered { get; private set; }
    public string Status { get; private set; } = ProductionOrderStatuses.Planned;
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyCollection<ProductionOrderAllocation> Allocations => _allocations.AsReadOnly();
    public IReadOnlyCollection<ProductionStage> Stages => _stages.AsReadOnly();

    public ProductionOrderAllocation AllocateWorkshop(Guid workshopId, decimal quantity)
    {
        EnsureAllocationAllowed();
        Guard.AgainstEmptyGuid(workshopId, nameof(workshopId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));

        var totalAllocated = _allocations.Sum(a => a.QuantityAllocated) + quantity;
        if (totalAllocated > QuantityOrdered)
        {
            throw new DomainException("Allocated quantity exceeds ordered quantity.");
        }

        var allocation = new ProductionOrderAllocation(Id, workshopId, quantity);
        _allocations.Add(allocation);
        return allocation;
    }

    public ProductionStage ScheduleStage(Guid workshopId, string stageType)
    {
        Guard.AgainstEmptyGuid(workshopId, nameof(workshopId));
        Guard.AgainstNullOrWhiteSpace(stageType, nameof(stageType));
        if (Status == ProductionOrderStatuses.Completed)
        {
            throw new DomainException("Cannot modify a completed production order.");
        }

        var stage = ProductionStage.Create(Id, workshopId, stageType);
        _stages.Add(stage);
        return stage;
    }

    public void Start()
    {
        if (Status != ProductionOrderStatuses.Planned)
        {
            throw new DomainException("Only planned orders can be started.");
        }

        if (!_stages.Any())
        {
            throw new DomainException("Cannot start production order without scheduled stages.");
        }

        Status = ProductionOrderStatuses.InProgress;
    }

    public void Complete()
    {
        if (Status != ProductionOrderStatuses.InProgress)
        {
            throw new DomainException("Only in-progress orders can be completed.");
        }

        if (_stages.Count == 0 || _stages.Any(stage => stage.Status != ProductionStageStatuses.Completed))
        {
            throw new DomainException("All stages must be completed before closing the order.");
        }

        var producedQuantity = _stages.Sum(stage => stage.QuantityOut);
        if (producedQuantity < QuantityOrdered)
        {
            throw new DomainException("Produced quantity is less than ordered quantity.");
        }

        Status = ProductionOrderStatuses.Completed;
    }

    private void EnsureAllocationAllowed()
    {
        if (Status != ProductionOrderStatuses.Planned && Status != ProductionOrderStatuses.InProgress)
        {
            throw new DomainException("Allocations are allowed only for planned or in-progress orders.");
        }
    }
}

public sealed class ProductionOrderAllocation : BaseEntity
{
    private ProductionOrderAllocation()
    {
    }

    internal ProductionOrderAllocation(Guid productionOrderId, Guid workshopId, decimal quantityAllocated)
    {
        Guard.AgainstEmptyGuid(productionOrderId, nameof(productionOrderId));
        Guard.AgainstEmptyGuid(workshopId, nameof(workshopId));
        Guard.AgainstNonPositive(quantityAllocated, nameof(quantityAllocated));

        ProductionOrderId = productionOrderId;
        WorkshopId = workshopId;
        QuantityAllocated = quantityAllocated;
    }

    public Guid ProductionOrderId { get; }
    public ProductionOrder? ProductionOrder { get; private set; }
    public Guid WorkshopId { get; }
    public Workshop? Workshop { get; private set; }
    public decimal QuantityAllocated { get; private set; }

    public void UpdateQuantity(decimal quantity)
    {
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        QuantityAllocated = quantity;
    }
}

public sealed class ProductionStage : BaseEntity
{
    private readonly List<WorkerAssignment> _assignments = new();

    private ProductionStage()
    {
    }

    private ProductionStage(Guid productionOrderId, Guid workshopId, string stageType)
    {
        Guard.AgainstEmptyGuid(productionOrderId, nameof(productionOrderId));
        Guard.AgainstEmptyGuid(workshopId, nameof(workshopId));
        Guard.AgainstNullOrWhiteSpace(stageType, nameof(stageType));

        ProductionOrderId = productionOrderId;
        WorkshopId = workshopId;
        StageType = stageType.Trim();
        Status = ProductionStageStatuses.Scheduled;
    }

    public static ProductionStage Create(Guid productionOrderId, Guid workshopId, string stageType)
    {
        return new ProductionStage(productionOrderId, workshopId, stageType);
    }

    public Guid ProductionOrderId { get; }
    public ProductionOrder? ProductionOrder { get; private set; }
    public Guid WorkshopId { get; }
    public Workshop? Workshop { get; private set; }
    public string StageType { get; private set; } = string.Empty;
    public decimal QuantityIn { get; private set; }
    public decimal QuantityOut { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime? RecordedAt { get; private set; }
    public string Status { get; private set; } = ProductionStageStatuses.Scheduled;
    public IReadOnlyCollection<WorkerAssignment> Assignments => _assignments.AsReadOnly();

    public void Start(decimal quantityIn, DateTime startedAt)
    {
        if (Status != ProductionStageStatuses.Scheduled)
        {
            throw new DomainException("Stage can only be started once.");
        }

        Guard.AgainstNonPositive(quantityIn, nameof(quantityIn));
        Guard.AgainstDefaultDate(startedAt, nameof(startedAt));

        QuantityIn = quantityIn;
        StartedAt = startedAt;
        RecordedAt = startedAt;
        Status = ProductionStageStatuses.InProgress;
    }

    public void Complete(decimal quantityOut, DateTime completedAt)
    {
        if (Status != ProductionStageStatuses.InProgress)
        {
            throw new DomainException("Stage must be in progress to complete.");
        }

        Guard.AgainstNonPositive(quantityOut, nameof(quantityOut));
        Guard.AgainstDefaultDate(completedAt, nameof(completedAt));
        if (quantityOut > QuantityIn)
        {
            throw new DomainException("Output quantity cannot exceed input quantity.");
        }

        QuantityOut = quantityOut;
        CompletedAt = completedAt;
        RecordedAt = completedAt;
        Status = ProductionStageStatuses.Completed;
    }

    public WorkerAssignment AssignWorker(Guid employeeId, decimal quantityAssigned, DateTime assignedAt)
    {
        if (Status != ProductionStageStatuses.InProgress)
        {
            throw new DomainException("Workers can only be assigned to in-progress stages.");
        }

        Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));
        Guard.AgainstNonPositive(quantityAssigned, nameof(quantityAssigned));
        Guard.AgainstDefaultDate(assignedAt, nameof(assignedAt));

        var existingAssignment = _assignments.FirstOrDefault(a => a.EmployeeId == employeeId);
        var totalAssigned = _assignments
            .Where(a => existingAssignment is null || a.Id != existingAssignment.Id)
            .Sum(a => a.QuantityAssigned);
        if (totalAssigned + quantityAssigned > QuantityIn)
        {
            throw new DomainException("Assigned quantity exceeds stage capacity.");
        }

        if (existingAssignment is not null)
        {
            existingAssignment.UpdateAssignedQuantity(quantityAssigned);
            return existingAssignment;
        }

        var assignment = WorkerAssignment.Create(Id, employeeId, quantityAssigned, assignedAt);
        _assignments.Add(assignment);
        return assignment;
    }
}

public sealed class WorkerAssignment : BaseEntity
{
    private WorkerAssignment()
    {
    }

    private WorkerAssignment(Guid productionStageId, Guid employeeId, decimal quantityAssigned, DateTime assignedAt)
    {
        Guard.AgainstEmptyGuid(productionStageId, nameof(productionStageId));
        Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));
        Guard.AgainstNonPositive(quantityAssigned, nameof(quantityAssigned));
        Guard.AgainstDefaultDate(assignedAt, nameof(assignedAt));

        ProductionStageId = productionStageId;
        EmployeeId = employeeId;
        QuantityAssigned = quantityAssigned;
        AssignedAt = assignedAt;
        Status = WorkerAssignmentStatuses.Assigned;
    }

    internal static WorkerAssignment Create(Guid productionStageId, Guid employeeId, decimal quantityAssigned, DateTime assignedAt)
    {
        return new WorkerAssignment(productionStageId, employeeId, quantityAssigned, assignedAt);
    }

    public Guid ProductionStageId { get; private set; }
    public ProductionStage? ProductionStage { get; private set; }
    public Guid EmployeeId { get; private set; }
    public Employee? Employee { get; private set; }
    public decimal QuantityAssigned { get; private set; }
    public decimal? QuantityCompleted { get; private set; }
    public DateTime AssignedAt { get; private set; }
    public string Status { get; private set; } = WorkerAssignmentStatuses.Assigned;

    public void UpdateAssignedQuantity(decimal quantityAssigned)
    {
        Guard.AgainstNonPositive(quantityAssigned, nameof(quantityAssigned));
        if (QuantityCompleted.HasValue && quantityAssigned < QuantityCompleted.Value)
        {
            throw new DomainException("Assigned quantity cannot be less than already completed work.");
        }

        QuantityAssigned = quantityAssigned;
    }

    public void StartWork()
    {
        if (Status != WorkerAssignmentStatuses.Assigned)
        {
            throw new DomainException("Assignment already started or finished.");
        }

        Status = WorkerAssignmentStatuses.InProgress;
    }

    public void CompleteWork(decimal quantityCompleted)
    {
        if (Status != WorkerAssignmentStatuses.InProgress)
        {
            throw new DomainException("Assignment must be in progress to complete.");
        }

        Guard.AgainstNonPositive(quantityCompleted, nameof(quantityCompleted));
        if (quantityCompleted > QuantityAssigned)
        {
            throw new DomainException("Completed quantity cannot exceed assigned quantity.");
        }

        QuantityCompleted = quantityCompleted;
        Status = WorkerAssignmentStatuses.Completed;
    }
}

public static class ProductionOrderStatuses
{
    public const string Planned = "Planned";
    public const string InProgress = "InProgress";
    public const string Completed = "Completed";
}

public static class ProductionStageStatuses
{
    public const string Scheduled = "Scheduled";
    public const string InProgress = "InProgress";
    public const string Completed = "Completed";
}

public static class WorkerAssignmentStatuses
{
    public const string Assigned = "Assigned";
    public const string InProgress = "InProgress";
    public const string Completed = "Completed";
}
