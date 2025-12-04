using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Operations;
using MyFactory.Domain.Entities.Shifts;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.Entities.Workshops;

namespace MyFactory.Domain.Entities.Production;

public sealed class ProductionOrder : BaseEntity
{
    private readonly List<ProductionOrderItem> _items = new();

    private ProductionOrder()
    {
    }

    public ProductionOrder(string orderNumber, DateOnly dueDate)
    {
        Guard.AgainstNullOrWhiteSpace(orderNumber, nameof(orderNumber));
        OrderNumber = orderNumber.Trim();
        DueDate = dueDate;
        Status = ProductionOrderStatus.Planned;
    }

    public string OrderNumber { get; private set; } = string.Empty;
    public DateOnly DueDate { get; private set; }
    public ProductionOrderStatus Status { get; private set; }
    public IReadOnlyCollection<ProductionOrderItem> Items => _items.AsReadOnly();

    public void Approve()
    {
        if (Status != ProductionOrderStatus.Planned)
        {
            throw new DomainException("Only planned orders can be approved.");
        }

        if (_items.Count == 0)
        {
            throw new DomainException("Cannot approve order without items.");
        }

        Status = ProductionOrderStatus.Approved;
    }

    public void Start()
    {
        if (Status != ProductionOrderStatus.Approved)
        {
            throw new DomainException("Order must be approved before starting.");
        }

        Status = ProductionOrderStatus.InProgress;
    }

    public void Complete()
    {
        if (Status != ProductionOrderStatus.InProgress)
        {
            throw new DomainException("Only in-progress orders can be completed.");
        }

        if (_items.Any(i => i.RemainingQuantity > 0))
        {
            throw new DomainException("Cannot complete order with remaining quantity.");
        }

        Status = ProductionOrderStatus.Completed;
    }

    public ProductionOrderItem AddItem(Guid specificationVersionId, decimal quantity)
    {
        var item = new ProductionOrderItem(Id, specificationVersionId, quantity);
        _items.Add(item);
        return item;
    }
}

public sealed class ProductionOrderItem : BaseEntity
{
    private readonly List<WorkOrder> _workOrders = new();

    private ProductionOrderItem()
    {
    }

    public ProductionOrderItem(Guid productionOrderId, Guid specificationVersionId, decimal quantity)
    {
        Guard.AgainstEmptyGuid(productionOrderId, nameof(productionOrderId));
        Guard.AgainstEmptyGuid(specificationVersionId, nameof(specificationVersionId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));

        ProductionOrderId = productionOrderId;
        SpecificationVersionId = specificationVersionId;
        Quantity = quantity;
    }

    public Guid ProductionOrderId { get; }
    public ProductionOrder? ProductionOrder { get; private set; }
    public Guid SpecificationVersionId { get; }
    public SpecificationVersion? SpecificationVersion { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal CompletedQuantity { get; private set; }
    public decimal RemainingQuantity => Quantity - CompletedQuantity;
    public IReadOnlyCollection<WorkOrder> WorkOrders => _workOrders.AsReadOnly();

    public WorkOrder CreateWorkOrder(Guid operationId, Guid workstationId, decimal plannedQuantity)
    {
        var workOrder = new WorkOrder(Id, operationId, workstationId, plannedQuantity);
        _workOrders.Add(workOrder);
        return workOrder;
    }

    public void RegisterCompletion(decimal quantity)
    {
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        if (CompletedQuantity + quantity > Quantity)
        {
            throw new DomainException("Completion exceeds ordered quantity.");
        }

        CompletedQuantity += quantity;
    }
}

public sealed class WorkOrder : BaseEntity
{
    private readonly List<WorkerAssignment> _assignments = new();

    private WorkOrder()
    {
    }

    public WorkOrder(Guid productionOrderItemId, Guid operationId, Guid workstationId, decimal plannedQuantity)
    {
        Guard.AgainstEmptyGuid(productionOrderItemId, nameof(productionOrderItemId));
        Guard.AgainstEmptyGuid(operationId, nameof(operationId));
        Guard.AgainstEmptyGuid(workstationId, nameof(workstationId));
        Guard.AgainstNonPositive(plannedQuantity, nameof(plannedQuantity));

        ProductionOrderItemId = productionOrderItemId;
        OperationId = operationId;
        WorkstationId = workstationId;
        PlannedQuantity = plannedQuantity;
        Status = WorkOrderStatus.Created;
    }

    public Guid ProductionOrderItemId { get; }
    public ProductionOrderItem? ProductionOrderItem { get; private set; }
    public Guid OperationId { get; }
    public Operation? Operation { get; private set; }
    public Guid WorkstationId { get; }
    public Workstation? Workstation { get; private set; }
    public decimal PlannedQuantity { get; private set; }
    public decimal CompletedQuantity { get; private set; }
    public WorkOrderStatus Status { get; private set; }
    public IReadOnlyCollection<WorkerAssignment> Assignments => _assignments.AsReadOnly();

    public void Start()
    {
        if (Status != WorkOrderStatus.Created)
        {
            throw new DomainException("Work order already started or finished.");
        }

        Status = WorkOrderStatus.InProgress;
    }

    public void Complete(decimal completedQuantity)
    {
        Guard.AgainstNonPositive(completedQuantity, nameof(completedQuantity));
        if (completedQuantity > PlannedQuantity)
        {
            throw new DomainException("Completed quantity cannot exceed plan.");
        }

        CompletedQuantity = completedQuantity;
        Status = WorkOrderStatus.Completed;
    }

    public WorkerAssignment AssignWorker(Guid employeeId, DateOnly assignedDate, Guid? shiftPlanId)
    {
        var assignment = new WorkerAssignment(Id, employeeId, assignedDate, shiftPlanId);
        _assignments.Add(assignment);
        return assignment;
    }
}

public sealed class WorkerAssignment : BaseEntity
{
    private WorkerAssignment()
    {
    }

    public WorkerAssignment(Guid workOrderId, Guid employeeId, DateOnly assignedDate, Guid? shiftPlanId)
    {
        Guard.AgainstEmptyGuid(workOrderId, nameof(workOrderId));
        Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));
        AssignedDate = assignedDate;
        WorkOrderId = workOrderId;
        EmployeeId = employeeId;
        ShiftPlanId = shiftPlanId;
        Status = WorkerAssignmentStatus.Assigned;
    }

    public Guid WorkOrderId { get; }
    public WorkOrder? WorkOrder { get; private set; }
    public Guid EmployeeId { get; }
    public Employee? Employee { get; private set; }
    public Guid? ShiftPlanId { get; private set; }
    public ShiftPlan? ShiftPlan { get; private set; }
    public DateOnly AssignedDate { get; private set; }
    public WorkerAssignmentStatus Status { get; private set; }

    public void Start()
    {
        if (Status != WorkerAssignmentStatus.Assigned)
        {
            throw new DomainException("Assignment already started or finished.");
        }

        Status = WorkerAssignmentStatus.InProgress;
    }

    public void Complete(decimal producedQuantity)
    {
        Guard.AgainstNonPositive(producedQuantity, nameof(producedQuantity));
        Status = WorkerAssignmentStatus.Completed;
        ProducedQuantity = producedQuantity;
    }

    public decimal? ProducedQuantity { get; private set; }
}

public enum ProductionOrderStatus
{
    Planned = 1,
    Approved = 2,
    InProgress = 3,
    Completed = 4
}

public enum WorkOrderStatus
{
    Created = 1,
    InProgress = 2,
    Completed = 3
}

public enum WorkerAssignmentStatus
{
    Assigned = 1,
    InProgress = 2,
    Completed = 3
}
