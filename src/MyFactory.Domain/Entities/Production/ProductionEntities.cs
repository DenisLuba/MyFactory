
using MyFactory.Domain.Common;
using MyFactory.Domain.Exceptions;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Orders;
using MyFactory.Domain.Entities.Organization;


namespace MyFactory.Domain.Entities.Production;

public enum ProductionOrderStatus
{
    New = 0, // Новый производственный заказ
    MaterialIssued = 1, // Выданы материалы
    Cutting = 2, // В раскрое
    Sewing = 3, // В пошиве
    Packaging = 4, // В упаковке
    Finished = 5, // Закончено
    Cancelled = 6 // Отклонено
}

public class ProductionOrderEntity : AuditableEntity
{
    // Properties mapped from ERD
    public int ProductionOrderNumber { get; private set; }
    public Guid SalesOrderItemId { get; private set; }
    public Guid DepartmentId { get; private set; }
    public decimal QtyPlanned { get; private set; }
    public decimal QtyFinished { get; private set; }
    public decimal QtyCut { get; private set; }
    public decimal QtySewn { get; private set; }
    public decimal QtyPacked { get; private set; }
    public ProductionOrderStatus Status { get; private set; }
    public Guid CreatedBy { get; private set; }

    // Navigation properties intentionally omitted: related entity types do not exist or are not required in the domain model per current context.
    public DepartmentEntity? Department { get; private set; }
    public SalesOrderItemEntity? SalesOrderItem { get; private set; }

    public IReadOnlyCollection<CuttingOperationEntity> CuttingOperations => _cuttingOperations;
    private readonly List<CuttingOperationEntity> _cuttingOperations = new();

    public IReadOnlyCollection<SewingOperationEntity> SewingOperations => _sewingOperations;
    private readonly List<SewingOperationEntity> _sewingOperations = new();

    public IReadOnlyCollection<PackagingOperationEntity> PackagingOperations => _packagingOperations;
    private readonly List<PackagingOperationEntity> _packagingOperations = new();


    public IReadOnlyCollection<InventoryMovementEntity>? InventoryMovements { get; private set; }
    public IReadOnlyCollection<FinishedGoodsEntity>? FinishedGoods { get; private set; }

    public ICollection<FinishedGoodsScrapEntity> FinishedGoodsScraps { get; private set; } = new List<FinishedGoodsScrapEntity>();

    public IReadOnlyCollection<ProductionOrderDepartmentEmployeeEntity> ProductionOrderDepartmentEmployees
        => _productionOrderDepartmentEmployees;
    private readonly List<ProductionOrderDepartmentEmployeeEntity> _productionOrderDepartmentEmployees = new List<ProductionOrderDepartmentEmployeeEntity>();

    // Constructor
    public ProductionOrderEntity(
        Guid salesOrderItemId,
        Guid departmentId,
        decimal qtyPlanned,
        Guid createdBy)
    {
        Guard.AgainstEmptyGuid(salesOrderItemId, nameof(salesOrderItemId));
        Guard.AgainstEmptyGuid(departmentId, nameof(departmentId));
        if (qtyPlanned <= 0)
            throw new DomainException($"{nameof(qtyPlanned)} must be positive.");
        Guard.AgainstEmptyGuid(createdBy, nameof(createdBy));

        SalesOrderItemId = salesOrderItemId;
        DepartmentId = departmentId;
        QtyPlanned = qtyPlanned;
        CreatedBy = createdBy;
        Status = ProductionOrderStatus.New;
        QtyFinished = 0;
        QtyCut = 0;
        QtySewn = 0;
        QtyPacked = 0;
    }

    private void SyncStageTotals(ProductionStage stage)
    {
        var totalCompleted = _productionOrderDepartmentEmployees
            .Where(x => x.Stage == stage)
            .Sum(x => x.CompletedQty);

        switch (stage)
        {
            case ProductionStage.Cutting:
                RecalculateCutQty(totalCompleted);
                break;

            case ProductionStage.Sewing:
                RecalculateSewnQty(totalCompleted);
                break;

            case ProductionStage.Packaging:
                RecalculatePackedQty(totalCompleted);
                break;

            default:
                throw new DomainException("Unknown production stage.");
        }
    }

    public ProductionOrderDepartmentEmployeeEntity AddStageAssignment(
        ProductionStage stage,
        EmployeeEntity employee,
        DateOnly workDate,
        decimal assignedQty,
        decimal completedQty)
    {
        //EnsureStageMatchesOrderStatus(stage);
        EnsureEmployeeCanWorkAtStage(employee, stage);
        var effectiveCompletedQty = stage == ProductionStage.Sewing
            ? 0m
            : completedQty;

        if (stage == ProductionStage.Sewing && completedQty != 0)
            throw new DomainException("Completed quantity for sewing assignments is derived from sewing operations and must be zero when creating an assignment.");

        EnsureStageCompletedCapacity(stage, effectiveCompletedQty, assignmentToExcludeId: null);

        var assignment = new ProductionOrderDepartmentEmployeeEntity(
            productionOrderId: Id,
            departmentId: DepartmentId,
            stage: stage,
            employeeId: employee.Id,
            workDate: workDate,
            assignedQty: assignedQty,
            completedQty: effectiveCompletedQty);

        _productionOrderDepartmentEmployees.Add(assignment);
        if (stage == ProductionStage.Sewing)
            SyncSewingAssignmentCompletion(assignment.Id);
        SyncStageTotals(stage);
        Touch();

        return assignment;
    }

    public void UpdateStageAssignment(
        Guid assignmentId,
        EmployeeEntity employee,
        DateOnly workDate,
        decimal assignedQty,
        decimal completedQty)
    {
        var assignment = _productionOrderDepartmentEmployees.FirstOrDefault(x => x.Id == assignmentId)
            ?? throw new DomainException("Stage assignment not found.");

        EnsureStageOpenForWork(assignment.Stage);
        EnsureEmployeeCanWorkAtStage(employee, assignment.Stage);
        var effectiveCompletedQty = completedQty;

        if (assignment.Stage == ProductionStage.Sewing)
        {
            effectiveCompletedQty = GetSewingCompletedQtyForAssignment(assignment.Id);

            if (completedQty != effectiveCompletedQty)
                throw new DomainException("Completed quantity for sewing assignments is derived from sewing operations and cannot be changed manually.");

            if (_sewingOperations.Any(x => x.AssignmentId == assignment.Id) && employee.Id != assignment.EmployeeId)
                throw new DomainException("Cannot change employee for a sewing assignment that already has sewing operations.");
        }

        EnsureStageCompletedCapacity(assignment.Stage, effectiveCompletedQty, assignment.Id);

        assignment.ChangeEmployee(employee.Id);
        assignment.UpdateAssignment(assignedQty, effectiveCompletedQty, workDate);

        if (assignment.Stage == ProductionStage.Sewing)
            SyncSewingAssignmentCompletion(assignment.Id);
        SyncStageTotals(assignment.Stage);
        Touch();
    }

    public void RemoveStageAssignment(Guid assignmentId)
    {
        var assignment = _productionOrderDepartmentEmployees.FirstOrDefault(x => x.Id == assignmentId)
            ?? throw new DomainException("Stage assignment not found.");

        EnsureStageOpenForWork(assignment.Stage);

        if (assignment.Stage == ProductionStage.Sewing && _sewingOperations.Any(x => x.AssignmentId == assignment.Id))
            throw new DomainException("Cannot remove sewing assignment that already has sewing operations.");

        _productionOrderDepartmentEmployees.Remove(assignment);
        SyncStageTotals(assignment.Stage);
        Touch();
    }

    public SewingOperationEntity RegisterSewingOperation(
        Guid assignmentId,
        Guid employeeId,
        decimal qtyPlanned,
        decimal qtySewn,
        decimal hoursWorked,
        DateOnly operationDate)
    {
        EnsureStageOpenForWork(ProductionStage.Sewing);

        var assignment = _productionOrderDepartmentEmployees.FirstOrDefault(x => x.Id == assignmentId)
            ?? throw new DomainException("Sewing assignment not found.");

        if (assignment.Stage != ProductionStage.Sewing)
            throw new DomainException("Specified assignment is not a sewing assignment.");

        if (assignment.EmployeeId != employeeId)
            throw new DomainException("Sewing operation employee must match assignment employee.");

        var assignmentCompletedQty = GetSewingCompletedQtyForAssignment(assignmentId);
        if (assignmentCompletedQty + qtySewn > assignment.AssignedQty)
            throw new DomainException("Completed sewing quantity cannot exceed assigned quantity.");

        if (QtySewn + qtySewn > QtyCut)
            throw new DomainException("Cannot sew more than cut quantity.");

        var operation = new SewingOperationEntity(
            productionOrderId: Id,
            employeeId: employeeId,
            assignmentId: assignmentId,
            qtyPlanned: qtyPlanned,
            qtySewn: qtySewn,
            hoursWorked: hoursWorked,
            operationDate: operationDate);

        _sewingOperations.Add(operation);
        SyncSewingAssignmentCompletion(assignmentId);
        SyncStageTotals(ProductionStage.Sewing);
        Touch();

        return operation;
    }

    public decimal GetAssignedQty(ProductionStage stage)
        => _productionOrderDepartmentEmployees
            .Where(x => x.Stage == stage)
            .Sum(x => x.AssignedQty);

    public decimal GetCompletedQty(ProductionStage stage)
        => _productionOrderDepartmentEmployees
            .Where(x => x.Stage == stage)
            .Sum(x => x.CompletedQty);

    public decimal GetRemainingQty(ProductionStage stage)
        => stage switch
        {
            ProductionStage.Cutting => QtyPlanned - GetCompletedQty(ProductionStage.Cutting),
            ProductionStage.Sewing => QtyCut - GetCompletedQty(ProductionStage.Sewing),
            ProductionStage.Packaging => QtySewn - GetCompletedQty(ProductionStage.Packaging),
            _ => throw new DomainException("Unknown production stage.")
        };

    private void EnsureStageOpenForWork(ProductionStage stage)
    {
        switch (stage)
        {
            case ProductionStage.Cutting:
                if (Status is not (ProductionOrderStatus.Cutting or ProductionOrderStatus.Sewing or ProductionOrderStatus.Packaging))
                    throw new DomainException($"Cannot manage assignments for stage {stage} while order is in status {Status}.");

                if (QtyCut >= QtyPlanned)
                    throw new DomainException("Cutting stage is already completed.");

                break;

            case ProductionStage.Sewing:
                if (Status is not (ProductionOrderStatus.Sewing or ProductionOrderStatus.Packaging))
                    throw new DomainException($"Cannot manage assignments for stage {stage} while order is in status {Status}.");

                if (QtyCut <= 0)
                    throw new DomainException("Cannot start sewing before at least one item is cut.");

                if (QtySewn >= QtyCut)
                    throw new DomainException("Sewing stage is already completed for currently cut quantity.");

                break;

            case ProductionStage.Packaging:
                if (Status != ProductionOrderStatus.Packaging)
                    throw new DomainException($"Cannot manage assignments for stage {stage} while order is in status {Status}.");

                if (QtySewn <= 0)
                    throw new DomainException("Cannot start packaging before at least one item is sewn.");

                if (QtyPacked >= QtySewn)
                    throw new DomainException("Packaging stage is already completed for currently sewn quantity.");

                break;

            default:
                throw new DomainException("Unknown production stage.");
        }
    }

    private void EnsureEmployeeCanWorkAtStage(EmployeeEntity employee, ProductionStage stage)
    {
        if (employee.DepartmentId != DepartmentId)
            throw new DomainException("Employee belongs to another department.");

        if (!employee.IsActive)
            throw new DomainException("Inactive employee cannot be assigned.");

        var isAllowed = stage switch
        {
            ProductionStage.Cutting => employee.Position?.CanCut == true,
            ProductionStage.Sewing => employee.Position?.CanSew == true,
            ProductionStage.Packaging => employee.Position?.CanPackage == true,
            _ => false
        };

        if (!isAllowed)
            throw new DomainException($"Employee is not allowed to work at stage {stage}.");
    }

    private void EnsureStageCompletedCapacity(
        ProductionStage stage,
        decimal newCompletedQty,
        Guid? assignmentToExcludeId)
    {
        if (newCompletedQty < 0)
            throw new DomainException("Completed quantity cannot be negative.");

        var completedWithoutCurrent = _productionOrderDepartmentEmployees
            .Where(x => x.Stage == stage && x.Id != assignmentToExcludeId)
            .Sum(x => x.CompletedQty);

        var totalCompleted = completedWithoutCurrent + newCompletedQty;
        var maxAllowed = stage switch
        {
            ProductionStage.Cutting => QtyPlanned,
            ProductionStage.Sewing => QtyCut,
            ProductionStage.Packaging => QtySewn,
            _ => throw new DomainException("Unknown production stage.")
        };

        if (totalCompleted > maxAllowed)
            throw new DomainException($"Total completed quantity for stage {stage} cannot exceed {maxAllowed}.");
    }

    private decimal GetSewingCompletedQtyForAssignment(Guid assignmentId)
        => _sewingOperations
            .Where(x => x.AssignmentId == assignmentId)
            .Sum(x => x.QtySewn);

    private void SyncSewingAssignmentCompletion(Guid assignmentId)
    {
        var assignment = _productionOrderDepartmentEmployees.FirstOrDefault(x => x.Id == assignmentId)
            ?? throw new DomainException("Sewing assignment not found.");

        if (assignment.Stage != ProductionStage.Sewing)
            throw new DomainException("Assignment is not a sewing assignment.");

        assignment.RegisterCompletion(GetSewingCompletedQtyForAssignment(assignmentId));
    }

    // State transition methods
    public void IssueMaterials()
    {
        if (Status != ProductionOrderStatus.New)
            throw new DomainException("Materials can only be issued from NEW status.");
        Status = ProductionOrderStatus.MaterialIssued;
        Touch();
    }

    public void StartCutting()
    {
        if (Status != ProductionOrderStatus.MaterialIssued)
            throw new DomainException("Cutting can only start after materials are issued.");
        Status = ProductionOrderStatus.Cutting;
        Touch();
    }

    public void StartSewing()
    {
        if (Status != ProductionOrderStatus.Cutting)
            throw new DomainException("Sewing can only start after cutting.");
        Status = ProductionOrderStatus.Sewing;
        Touch();
    }

    public void StartPackaging()
    {
        if (Status != ProductionOrderStatus.Sewing)
            throw new DomainException("Packaging can only start after sewing.");
        Status = ProductionOrderStatus.Packaging;
        Touch();
    }

    public void FinishOrder()
    {
        if (Status != ProductionOrderStatus.Packaging)
            throw new DomainException("Order can only be finished after packaging.");
        if (QtyFinished < QtyPlanned)
            throw new DomainException("Cannot finish order: not all items produced.");
        Status = ProductionOrderStatus.Finished;
        Touch();
    }

    public void CancelOrder()
    {
        if (Status == ProductionOrderStatus.Finished)
            throw new DomainException("Cannot cancel a finished order.");
        Status = ProductionOrderStatus.Cancelled;
        Touch();
    }

    // Incremental quantity methods
    public void AddCut(decimal qty)
    {
        if (qty <= 0)
            throw new DomainException($"{nameof(qty)} must be positive.");
        if (Status != ProductionOrderStatus.Cutting)
            throw new DomainException("Can only add cut quantity during Cutting stage.");
        if (QtyCut + qty > QtyPlanned)
            throw new DomainException("Cannot cut more than planned quantity.");
        QtyCut += qty;
        Touch();
    }

    public void AddSewn(decimal qty)
    {
        if (qty <= 0)
            throw new DomainException($"{nameof(qty)} must be positive.");
        if (Status != ProductionOrderStatus.Sewing)
            throw new DomainException("Can only add sewn quantity during Sewing stage.");
        if (QtySewn + qty > QtyCut)
            throw new DomainException("Cannot sew more than cut quantity.");
        QtySewn += qty;
        Touch();
    }

    public void AddPacked(decimal qty)
    {
        if (qty <= 0)
            throw new DomainException($"{nameof(qty)} must be positive.");
        if (Status != ProductionOrderStatus.Packaging)
            throw new DomainException("Can only add packed quantity during Packaging stage.");
        if (QtyPacked + qty > QtySewn)
            throw new DomainException("Cannot pack more than sewn quantity.");
        QtyPacked += qty;
        Touch();
    }

    public void AddFinished(decimal qty)
    {
        if (qty <= 0)
            throw new DomainException($"{nameof(qty)} must be positive.");
        if (Status != ProductionOrderStatus.Packaging)
            throw new DomainException("Can only add finished quantity during Packaging stage.");
        if (QtyFinished + qty > QtyPlanned)
            throw new DomainException("Cannot finish more than planned quantity.");
        if (QtyFinished + qty > QtyPacked)
            throw new DomainException("Cannot finish more than packed quantity.");
        QtyFinished += qty;
        Touch();
    }

    public void Update(Guid departmentId, decimal qtyPlanned)
    {
        if (Status != ProductionOrderStatus.New)
            throw new DomainException("Only new production orders can be updated.");
        Guard.AgainstEmptyGuid(departmentId, nameof(departmentId));
        Guard.AgainstNonPositive(qtyPlanned, nameof(QtyPlanned));

        if (qtyPlanned < QtyPlanned)
            throw new DomainException("Cannot reduce planned quantity below already planned amount.");

        DepartmentId = departmentId;
        QtyPlanned = qtyPlanned;

        Touch();
    }

    public void RecalculateCutQty(decimal totalCutQty)
    {
        if (totalCutQty < 0)
            throw new DomainException($"{nameof(totalCutQty)} cannot be negative.");
        if (totalCutQty > QtyPlanned)
            throw new DomainException("Cannot cut more than planned quantity.");
        QtyCut = totalCutQty;
        Touch();
    }

    public void RecalculateSewnQty(decimal totalSewnQty)
    {
        if (totalSewnQty < 0)
            throw new DomainException($"{nameof(totalSewnQty)} cannot be negative.");
        if (totalSewnQty > QtyCut)
            throw new DomainException("Cannot sew more than cut quantity.");
        QtySewn = totalSewnQty;
        Touch();
    }

    public void RecalculatePackedQty(decimal totalPackedQty)
    {
        if (totalPackedQty < 0)
            throw new DomainException($"{nameof(totalPackedQty)} cannot be negative.");
        if (totalPackedQty > QtySewn)
            throw new DomainException("Cannot pack more than sewn quantity.");
        QtyPacked = totalPackedQty;
        Touch();
    }

    public void RemoveCut(decimal qtyCut)
    {
        if (qtyCut <= 0)
            throw new DomainException($"{nameof(qtyCut)} must be positive.");
        if (QtyCut - qtyCut < 0)
            throw new DomainException("Cannot have negative cut quantity.");
        QtyCut -= qtyCut;
        Touch();
    }

    public void RemoveSewn(decimal qtySewn)
    {
        if (qtySewn <= 0)
            throw new DomainException($"{nameof(qtySewn)} must be positive.");
        if (QtySewn - qtySewn < 0)
            throw new DomainException("Cannot have negative sewn quantity.");
        QtySewn -= qtySewn;
        Touch();
    }

    public void RemovePacked(decimal qtyPacked)
    {
        if (qtyPacked <= 0)
            throw new DomainException($"{nameof(qtyPacked)} must be positive.");
        if (QtyPacked - qtyPacked < 0)
            throw new DomainException("Cannot have negative packed quantity.");
        QtyPacked -= qtyPacked;
        Touch();
    }
}

public enum ProductionStage
{
    Cutting = 1,
    Sewing = 2,
    Packaging = 3
}

public class ProductionOrderDepartmentEmployeeEntity : AuditableEntity
{
    public Guid ProductionOrderId { get; private set; }
    public Guid DepartmentId { get; private set; }
    public ProductionStage Stage { get; private set; }
    public Guid EmployeeId { get; private set; }
    public DateOnly WorkDate { get; private set; }

    public decimal AssignedQty { get; private set; }
    public decimal CompletedQty { get; private set; }

    public ProductionOrderEntity? ProductionOrder { get; private set; }
    public EmployeeEntity? Employee { get; private set; }
    public DepartmentEntity? Department { get; private set; }
    public IReadOnlyCollection<SewingOperationEntity> SewingOperations { get; private set; } = new List<SewingOperationEntity>();

    protected ProductionOrderDepartmentEmployeeEntity() { }

    public ProductionOrderDepartmentEmployeeEntity(
        Guid productionOrderId,
        Guid departmentId,
        ProductionStage stage,
        Guid employeeId,
        DateOnly workDate,
        decimal assignedQty,
        decimal completedQty)
    {
        Guard.AgainstEmptyGuid(productionOrderId, nameof(productionOrderId));
        Guard.AgainstEmptyGuid(departmentId, nameof(departmentId));
        Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));

        if (workDate == default)
            throw new DomainException("Work date cannot be empty.");

        ValidateQuantities(assignedQty, completedQty);

        ProductionOrderId = productionOrderId;
        DepartmentId = departmentId;
        Stage = stage;
        EmployeeId = employeeId;
        WorkDate = workDate;
        AssignedQty = assignedQty;
        CompletedQty = completedQty;
    }

    public void UpdateAssignment(decimal assignedQty, decimal completedQty, DateOnly workDate)
    {
        if (workDate == default)
            throw new DomainException("Work date cannot be empty.");

        ValidateQuantities(assignedQty, completedQty);

        AssignedQty = assignedQty;
        CompletedQty = completedQty;
        WorkDate = workDate;

        Touch();
    }

    public void ChangeEmployee(Guid employeeId)
    {
        Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));
        EmployeeId = employeeId;
        Touch();
    }

    public void RegisterCompletion(decimal completedQty)
    {
        if (completedQty < 0)
            throw new DomainException("Completed quantity cannot be negative.");

        if (completedQty > AssignedQty)
            throw new DomainException("Completed quantity cannot exceed assigned quantity.");

        CompletedQty = completedQty;
        Touch();
    }

    private static void ValidateQuantities(decimal assignedQty, decimal completedQty)
    {
        if (assignedQty < 0)
            throw new DomainException("Assigned quantity cannot be negative.");

        if (completedQty < 0)
            throw new DomainException("Completed quantity cannot be negative.");

        if (completedQty > assignedQty)
            throw new DomainException("Completed quantity cannot exceed assigned quantity.");
    }
}

public class CuttingOperationEntity : AuditableEntity
{
    // Properties mapped from ERD
    public Guid ProductionOrderId { get; private set; }
    public Guid EmployeeId { get; private set; }
    public decimal QtyPlanned { get; private set; }
    public decimal QtyCut { get; private set; }
    public DateOnly OperationDate { get; private set; }

    // Navigation property
    public ProductionOrderEntity? ProductionOrder { get; private set; }
    public EmployeeEntity? Employee { get; private set; }

    // Constructor
    public CuttingOperationEntity(Guid productionOrderId, Guid employeeId, decimal qtyPlanned, decimal qtyCut, DateOnly operationDate)
    {
        Guard.AgainstEmptyGuid(productionOrderId, nameof(productionOrderId));
        Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));
        Guard.AgainstNonPositive(qtyPlanned, nameof(qtyPlanned));
        Guard.AgainstNonPositive(qtyCut, nameof(qtyCut));
        Guard.AgainstDefaultDate(operationDate, nameof(operationDate));

        ProductionOrderId = productionOrderId;
        EmployeeId = employeeId;
        QtyPlanned = qtyPlanned;
        QtyCut = qtyCut;
        OperationDate = operationDate;
    }

    // No business methods specified in ERD/spec for this entity
}

public class SewingOperationEntity : AuditableEntity
{
    // Properties mapped from ERD
    public Guid ProductionOrderId { get; private set; }
    public Guid EmployeeId { get; private set; }
    public Guid AssignmentId { get; private set; }
    public decimal QtyPlanned { get; private set; }
    public decimal QtySewn { get; private set; }
    public decimal HoursWorked { get; private set; }
    public DateOnly OperationDate { get; private set; }

    // Navigation property 
    public ProductionOrderEntity? ProductionOrder { get; private set; }
    public EmployeeEntity? Employee { get; private set; }
    public ProductionOrderDepartmentEmployeeEntity? Assignment { get; private set; }

    // Constructor
    public SewingOperationEntity(Guid productionOrderId, Guid employeeId, Guid assignmentId, decimal qtyPlanned, decimal qtySewn, decimal hoursWorked, DateOnly operationDate)
    {
        Guard.AgainstEmptyGuid(productionOrderId, nameof(productionOrderId));
        Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));
        Guard.AgainstEmptyGuid(assignmentId, nameof(assignmentId));
        Guard.AgainstNonPositive(qtyPlanned, nameof(qtyPlanned));
        Guard.AgainstNonPositive(qtySewn, nameof(qtySewn));
        Guard.AgainstNonPositive(hoursWorked, nameof(hoursWorked));
        Guard.AgainstDefaultDate(operationDate, nameof(operationDate));

        ProductionOrderId = productionOrderId;
        EmployeeId = employeeId;
        AssignmentId = assignmentId;
        QtyPlanned = qtyPlanned;
        QtySewn = qtySewn;
        HoursWorked = hoursWorked;
        OperationDate = operationDate;
    }

    // No business methods specified in ERD/spec for this entity
}

public class PackagingOperationEntity : AuditableEntity
{
    // Properties mapped from ERD
    public Guid ProductionOrderId { get; private set; }
    public Guid EmployeeId { get; private set; }
    public decimal QtyPlanned { get; private set; }
    public decimal QtyPacked { get; private set; }
    public DateOnly OperationDate { get; private set; }

    // Navigation property
    public ProductionOrderEntity? ProductionOrder { get; private set; }
    public EmployeeEntity? Employee { get; private set; }

    // Constructor
    public PackagingOperationEntity(Guid productionOrderId, Guid employeeId, decimal qtyPlanned, decimal qtyPacked, DateOnly operationDate)
    {
        Guard.AgainstEmptyGuid(productionOrderId, nameof(productionOrderId));
        Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));
        Guard.AgainstNonPositive(qtyPlanned, nameof(qtyPlanned));
        Guard.AgainstNonPositive(qtyPacked, nameof(qtyPacked));
        Guard.AgainstDefaultDate(operationDate, nameof(operationDate));

        ProductionOrderId = productionOrderId;
        EmployeeId = employeeId;
        QtyPlanned = qtyPlanned;
        QtyPacked = qtyPacked;
        OperationDate = operationDate;
    }

    // No business methods specified in ERD/spec for this entity
}
