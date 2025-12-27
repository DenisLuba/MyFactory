
using MyFactory.Domain.Common;
using MyFactory.Domain.Exceptions;
using MyFactory.Domain.Entities.Inventory;


namespace MyFactory.Domain.Entities.Production;

public enum ProductionOrderStatus
{
	New = 0,
	MaterialIssued = 1,
	Cutting = 2,
	Sewing = 3,
	Packaging = 4,
	Finished = 5,
	Cancelled = 6
}

public class ProductionOrderEntity : AuditableEntity
{
	// Properties mapped from ERD
	public string ProductionOrderNumber { get; private set; }
	public Guid SalesOrderItemId { get; private set; }
	public Guid DepartmentId { get; private set; }
	public int QtyPlanned { get; private set; }
	public int QtyFinished { get; private set; }
	public int QtyCut { get; private set; }
	public int QtySewn { get; private set; }
	public int QtyPacked { get; private set; }
	public ProductionOrderStatus Status { get; private set; }
	public Guid CreatedBy { get; private set; }

	// Navigation properties intentionally omitted: related entity types do not exist or are not required in the domain model per current context.

	public IReadOnlyCollection<CuttingOperationEntity> CuttingOperations => _cuttingOperations;
	private readonly List<CuttingOperationEntity> _cuttingOperations = new();

	public IReadOnlyCollection<SewingOperationEntity> SewingOperations => _sewingOperations;
	private readonly List<SewingOperationEntity> _sewingOperations = new();

	public IReadOnlyCollection<PackagingOperationEntity> PackagingOperations => _packagingOperations;
	private readonly List<PackagingOperationEntity> _packagingOperations = new();


	public IReadOnlyCollection<InventoryMovementEntity>? InventoryMovements { get; private set; }
	public IReadOnlyCollection<FinishedGoodsEntity>? FinishedGoods { get; private set; }


	// Constructor
	public ProductionOrderEntity(
		string productionOrderNumber,
		Guid salesOrderItemId,
		Guid departmentId,
		int qtyPlanned,
		Guid createdBy)
	{
		Guard.AgainstNullOrWhiteSpace(productionOrderNumber, nameof(productionOrderNumber));
		Guard.AgainstEmptyGuid(salesOrderItemId, nameof(salesOrderItemId));
		Guard.AgainstEmptyGuid(departmentId, nameof(departmentId));
		if (qtyPlanned <= 0)
			throw new DomainException($"{nameof(qtyPlanned)} must be positive.");
		Guard.AgainstEmptyGuid(createdBy, nameof(createdBy));

		ProductionOrderNumber = productionOrderNumber;
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
	public void AddCut(int qty)
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

	public void AddSewn(int qty)
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

	public void AddPacked(int qty)
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

	public void AddFinished(int qty)
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

	public void Update(Guid departmentId, int qtyPlanned)
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

	public void RecalculateCutQty(int totalCutQty)
	{
        if (totalCutQty < 0)
            throw new DomainException($"{nameof(totalCutQty)} must be positive.");
        if (Status != ProductionOrderStatus.Cutting)
            throw new DomainException("Can only recalculate cut quantity during Cutting stage.");
        if (totalCutQty > QtyPlanned)
            throw new DomainException("Cannot cut more than cut quantity.");
        QtyCut = totalCutQty;
		Touch();
	}

	public void RecalculateSewnQty(int totalSewnQty)
	{
		if (totalSewnQty < 0)
			throw new DomainException($"{nameof(totalSewnQty)} must be positive.");
		if (Status != ProductionOrderStatus.Sewing)
			throw new DomainException("Can only add sewn quantity during Sewing stage.");
		if (totalSewnQty > QtyCut)
			throw new DomainException("Cannot sew more than cut quantity.");
        QtySewn = totalSewnQty;
		Touch();
	}

	public void RecalculatePackedQty(int totalPackedQty)
	{
		if (totalPackedQty < 0)
			throw new DomainException($"{nameof(totalPackedQty)} must be positive.");
		if (Status != ProductionOrderStatus.Packaging)
			throw new DomainException("Can only add packed quantity during Packaging stage.");
		if (totalPackedQty > QtySewn)
			throw new DomainException("Cannot pack more than sewn quantity.");
        QtyPacked = totalPackedQty;
		Touch();
    }

    public void RemoveCut(int qtyCut)
    {
        if (qtyCut <= 0)
			throw new DomainException($"{nameof(qtyCut)} must be positive.");
		if (QtyCut - qtyCut < 0)
			throw new DomainException("Cannot have negative cut quantity.");
		QtyCut -= qtyCut;
		Touch();
    }

	public void RemoveSewn(int qtySewn)
	{
		if (qtySewn <= 0)
			throw new DomainException($"{nameof(qtySewn)} must be positive.");
		if (QtySewn - qtySewn < 0)
			throw new DomainException("Cannot have negative sewn quantity.");
		QtySewn -= qtySewn;
		Touch();
    }

	public void RemovePacked(int qtyPacked)
	{ 		
		if (qtyPacked <= 0)
			throw new DomainException($"{nameof(qtyPacked)} must be positive.");
		if (QtyPacked - qtyPacked < 0)
			throw new DomainException("Cannot have negative packed quantity.");
		QtyPacked -= qtyPacked;
		Touch();
    }
}

public class CuttingOperationEntity : AuditableEntity
{
	// Properties mapped from ERD
	public Guid ProductionOrderId { get; private set; }
	public Guid EmployeeId { get; private set; }
	public int QtyPlanned { get; private set; }
    public int QtyCut { get; private set; }
	public DateOnly OperationDate { get; private set; }

	// Navigation property stubs (types must exist elsewhere in the domain layer)
	// public ProductionOrderEntity? ProductionOrder { get; private set; }
	// public EmployeeEntity? Employee { get; private set; }

	// Constructor
	public CuttingOperationEntity(Guid productionOrderId, Guid employeeId, int qtyPlanned, int qtyCut, DateOnly operationDate)
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
    public int QtyPlanned { get; private set; }
    public int QtySewn { get; private set; }
	public decimal HoursWorked { get; private set; }
	public DateOnly OperationDate { get; private set; }

	// Navigation property stubs (types must exist elsewhere in the domain layer)
	// public ProductionOrderEntity? ProductionOrder { get; private set; }
	// public EmployeeEntity? Employee { get; private set; }

	// Constructor
	public SewingOperationEntity(Guid productionOrderId, Guid employeeId, int qtyPlanned, int qtySewn, decimal hoursWorked, DateOnly operationDate)
	{
		Guard.AgainstEmptyGuid(productionOrderId, nameof(productionOrderId));
		Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));
		Guard.AgainstNonPositive(qtyPlanned, nameof(qtyPlanned));
		Guard.AgainstNonPositive(qtySewn, nameof(qtySewn));
		Guard.AgainstNonPositive(hoursWorked, nameof(hoursWorked));
		Guard.AgainstDefaultDate(operationDate, nameof(operationDate));

		ProductionOrderId = productionOrderId;
		EmployeeId = employeeId;
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
    public int QtyPlanned { get; private set; }
    public int QtyPacked { get; private set; }
	public DateOnly OperationDate { get; private set; }

	// Navigation property stubs (types must exist elsewhere in the domain layer)
	// public ProductionOrderEntity? ProductionOrder { get; private set; }
	// public EmployeeEntity? Employee { get; private set; }

	// Constructor
	public PackagingOperationEntity(Guid productionOrderId, Guid employeeId, int qtyPlanned, int qtyPacked, DateOnly operationDate)
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

