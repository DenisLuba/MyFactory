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

public sealed class ProductionOrderCompleted
{
    public Guid ProductionOrderId { get; }
    public DateTime CompletedAt { get; }

    public ProductionOrderCompleted(Guid productionOrderId, DateTime completedAt)
    {
        ProductionOrderId = productionOrderId;
        CompletedAt = completedAt;
    }
}

public sealed class WorkerAssignmentCompleted
{
    public Guid AssignmentId { get; }
    public Guid EmployeeId { get; }
    public DateTime CompletedAt { get; }

    public WorkerAssignmentCompleted(Guid assignmentId, Guid employeeId, DateTime completedAt)
    {
        AssignmentId = assignmentId;
        EmployeeId = employeeId;
        CompletedAt = completedAt;
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

/// <summary>
/// Production orders aggregate.
/// NOTE: decimal precision/scale should be configured in ORM mapping (e.g. decimal(18,4)).
/// TODO: consider adding application-level orchestration for TimesheetEntry creation when stages complete.
/// </summary>
public sealed class ProductionOrder : BaseEntity
{
    public const int OrderNumberMaxLength = 100;

    private readonly List<ProductionOrderAllocation> _allocations = new();
    private readonly List<ProductionStage> _stages = new();

    private ProductionOrder()
    {
    }

    private ProductionOrder(string orderNumber, Guid specificationId, decimal quantityOrdered, DateOnly createdAt)
    {
        Guard.AgainstNullOrWhiteSpace(orderNumber, nameof(orderNumber));
        var onTrim = orderNumber.Trim();
        if (onTrim.Length > OrderNumberMaxLength)
        {
            throw new DomainException($"Order number cannot exceed {OrderNumberMaxLength} characters.");
        }

        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstNonPositive(quantityOrdered, nameof(quantityOrdered));
        Guard.AgainstDefaultDate(createdAt, nameof(createdAt));

        OrderNumber = onTrim;
        SpecificationId = specificationId;
        QuantityOrdered = quantityOrdered;
        CreatedAt = createdAt;
        Status = ProductionOrderStatuses.Planned;
    }

    public static ProductionOrder Create(string orderNumber, Guid specificationId, decimal quantityOrdered, DateOnly createdAt)
    {
        return new ProductionOrder(orderNumber, specificationId, quantityOrdered, createdAt);
    }

    public string OrderNumber { get; private set; } = string.Empty;
    public Guid SpecificationId { get; private set; }
    public Specification? Specification { get; private set; }
    public decimal QuantityOrdered { get; private set; }
    public string Status { get; private set; } = ProductionOrderStatuses.Planned;
    public DateOnly CreatedAt { get; private set; }
    public IReadOnlyCollection<ProductionOrderAllocation> Allocations => _allocations.AsReadOnly();
    public IReadOnlyCollection<ProductionStage> Stages => _stages.AsReadOnly();

    public ProductionOrderAllocation AllocateWorkshop(Guid workshopId, decimal quantity)
    {
        EnsureAllocationAllowed();
        Guard.AgainstEmptyGuid(workshopId, nameof(workshopId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));

        // Ensure total allocated does not exceed ordered quantity
        var totalAllocated = _allocations.Sum(a => a.QuantityAllocated) + quantity;
        if (totalAllocated > QuantityOrdered)
        {
            throw new DomainException("Allocated quantity exceeds ordered quantity.");
        }

        var allocation = new ProductionOrderAllocation(Id, workshopId, quantity);
        // keep in-memory graph consistent
        allocation.ProductionOrder = this;
        _allocations.Add(allocation);
        return allocation;
    }

    public void UpdateAllocation(Guid allocationId, decimal newQuantity)
    {
        Guard.AgainstEmptyGuid(allocationId, nameof(allocationId));
        Guard.AgainstNonPositive(newQuantity, nameof(newQuantity));

        var allocation = _allocations.FirstOrDefault(a => a.Id == allocationId)
            ?? throw new DomainException("Allocation not found.");

        var totalOther = _allocations.Where(a => a.Id != allocationId).Sum(a => a.QuantityAllocated);
        if (totalOther + newQuantity > QuantityOrdered)
        {
            throw new DomainException("Updated allocation would exceed ordered quantity.");
        }

        allocation.UpdateQuantity(newQuantity);
    }

    public void AttachAllocation(ProductionOrderAllocation allocation)
    {
        Guard.AgainstNull(allocation, nameof(allocation));
        if (allocation.ProductionOrderId != Id)
        {
            throw new DomainException("Allocation does not belong to this production order.");
        }
        if (allocation.ProductionOrder != null && allocation.ProductionOrder.Id != Id)
        {
            throw new DomainException("Allocation navigation mismatch.");
        }

        if (_allocations.Exists(a => a.Id == allocation.Id))
        {
            return;
        }

        allocation.ProductionOrder = this;
        _allocations.Add(allocation);
    }

    public void DetachAllocation(ProductionOrderAllocation allocation)
    {
        Guard.AgainstNull(allocation, nameof(allocation));
        var index = _allocations.FindIndex(a => a.Id == allocation.Id);
        if (index == -1)
        {
            return;
        }
        _allocations.RemoveAt(index);
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
        stage.ProductionOrder = this;
        _stages.Add(stage);
        return stage;
    }

    public void AttachStage(ProductionStage stage)
    {
        Guard.AgainstNull(stage, nameof(stage));
        if (stage.ProductionOrderId != Id)
        {
            throw new DomainException("Stage does not belong to this production order.");
        }
        if (stage.ProductionOrder != null && stage.ProductionOrder.Id != Id)
        {
            throw new DomainException("Stage navigation mismatch.");
        }

        if (_stages.Exists(s => s.Id == stage.Id))
        {
            return;
        }

        stage.ProductionOrder = this;
        _stages.Add(stage);
    }

    public void DetachStage(ProductionStage stage)
    {
        Guard.AgainstNull(stage, nameof(stage));
        var index = _stages.FindIndex(s => s.Id == stage.Id);
        if (index == -1)
        {
            return;
        }
        _stages.RemoveAt(index);
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

        // TODO: If business requires, transition first stage to InProgress here or trigger domain event.
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

        // TODO: publish domain event ProductionOrderCompleted { OrderId = this.Id } so application layer can create TimesheetEntries.
        AddDomainEvent(new ProductionOrderCompleted(Id, DateTime.UtcNow));
    }

    private void EnsureAllocationAllowed()
    {
        if (Status != ProductionOrderStatuses.Planned && Status != ProductionOrderStatuses.InProgress)
        {
            throw new DomainException("Allocations are allowed only for planned or in-progress orders.");
        }
    }

    public void StartStage(Guid stageId, decimal quantityIn)
    {
        Guard.AgainstEmptyGuid(stageId, nameof(stageId));
        Guard.AgainstNonPositive(quantityIn, nameof(quantityIn));

        if (Status == ProductionOrderStatuses.Completed)
        {
            throw new DomainException("Cannot start a stage on a completed production order.");
        }

        var stage = _stages.FirstOrDefault(s => s.Id == stageId)
            ?? throw new DomainException("Production stage not found in this order.");

        if (stage.Status != ProductionStageStatuses.Scheduled)
        {
            throw new DomainException("Only scheduled stages can be started.");
        }

        // Find allocation for the workshop
        var allocation = _allocations.FirstOrDefault(a => a.WorkshopId == stage.WorkshopId)
            ?? throw new DomainException("No allocation found for the workshop of this stage.");

        // Calculate already reserved/consumed QuantityIn for this workshop across other stages
        var totalAssignedForWorkshop = _stages
            .Where(s => s.WorkshopId == stage.WorkshopId && s.Id != stage.Id)
            .Sum(s => s.QuantityIn);

        if (totalAssignedForWorkshop + quantityIn > allocation.QuantityAllocated)
        {
            throw new DomainException("Starting this stage with the requested quantity exceeds allocated capacity for the workshop.");
        }

        // Start stage with current UTC time
        stage.Start(quantityIn, DateTime.UtcNow);
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

    public Guid ProductionOrderId { get; private set; }
    public ProductionOrder? ProductionOrder { get; internal set; }
    public Guid WorkshopId { get; private set; }
    public Workshop? Workshop { get; internal set; }
    public decimal QuantityAllocated { get; private set; }

    internal void UpdateQuantity(decimal quantity)
    {
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        // Note: caller (ProductionOrder) must ensure overall allocations do not exceed ordered quantity.
        QuantityAllocated = quantity;
    }
}

public sealed class ProductionStage : BaseEntity
{
    private readonly List<WorkerAssignment> _assignments = new();

    private ProductionStage()
    {
    }

    public const int StageTypeMaxLength = 100;

    private ProductionStage(Guid productionOrderId, Guid workshopId, string stageType)
    {
        Guard.AgainstEmptyGuid(productionOrderId, nameof(productionOrderId));
        Guard.AgainstEmptyGuid(workshopId, nameof(workshopId));
        Guard.AgainstNullOrWhiteSpace(stageType, nameof(stageType));
        var stTrim = stageType.Trim();
        if (stTrim.Length > StageTypeMaxLength)
        {
            throw new DomainException($"Stage type cannot exceed {StageTypeMaxLength} characters.");
        }

        ProductionOrderId = productionOrderId;
        WorkshopId = workshopId;
        StageType = stTrim;
        Status = ProductionStageStatuses.Scheduled;
    }

    public static ProductionStage Create(Guid productionOrderId, Guid workshopId, string stageType)
    {
        return new ProductionStage(productionOrderId, workshopId, stageType);
    }

    public Guid ProductionOrderId { get; private set; }
    public ProductionOrder? ProductionOrder { get; internal set; }
    public Guid WorkshopId { get; private set; }
    public Workshop? Workshop { get; internal set; }
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

        // TODO: validate quantityIn against allocations for this workshop in ProductionOrder (application/service responsibility).
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
        assignment.ProductionStage = this;
        _assignments.Add(assignment);
        return assignment;
    }

    internal void AttachAssignment(WorkerAssignment assignment)
    {
        Guard.AgainstNull(assignment, nameof(assignment));
        if (assignment.ProductionStageId != Id)
        {
            throw new DomainException("Assignment does not belong to this stage.");
        }
        if (assignment.ProductionStage != null && assignment.ProductionStage.Id != Id)
        {
            throw new DomainException("Assignment navigation mismatch.");
        }

        if (_assignments.Any(a => a.Id == assignment.Id))
        {
            return;
        }

        assignment.ProductionStage = this;
        _assignments.Add(assignment);
    }

    internal void DetachAssignment(WorkerAssignment assignment)
    {
        Guard.AgainstNull(assignment, nameof(assignment));
        var index = _assignments.FindIndex(a => a.Id == assignment.Id);
        if (index == -1)
        {
            return;
        }
        _assignments.RemoveAt(index);
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
    public ProductionStage? ProductionStage { get; internal set; }
    public Guid EmployeeId { get; private set; }
    public Employee? Employee { get; internal set; }
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

        // TODO: application layer should create or update TimesheetEntry for the employee here.
        // TODO: publish domain event WorkerAssignmentCompleted { AssignmentId = this.Id } to notify other parts of the system.
        AddDomainEvent(new WorkerAssignmentCompleted(Id, EmployeeId, DateTime.UtcNow));
    }
}
