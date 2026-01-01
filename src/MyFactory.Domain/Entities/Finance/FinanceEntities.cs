using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Products;
using MyFactory.Domain.Exceptions;

namespace MyFactory.Domain.Entities.Finance;

public class TimesheetEntity : AuditableEntity
{
	// Properties mapped from ERD
	public Guid EmployeeId { get; private set; }
	public Guid DepartmentId { get; private set; }
	public DateOnly WorkDate { get; private set; }
	public decimal HoursWorked { get; private set; }
	public string? Comment { get; private set; }

    // Navigation property stubs (types must exist elsewhere in the domain layer)
    // public EmployeeEntity? Employee { get; private set; }
    // public DepartmentEntity? Department { get; private set; }

    // Constructor
    public TimesheetEntity(Guid employeeId, Guid departmentId, DateOnly workDate, decimal hoursWorked, string? comment = null)
	{
		Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));
		Guard.AgainstEmptyGuid(departmentId, nameof(departmentId));
		Guard.AgainstDefaultDate(workDate, nameof(workDate));
		if (hoursWorked < 0)
			throw new DomainException($"{nameof(hoursWorked)} cannot be negative.");

		EmployeeId = employeeId;
		DepartmentId = departmentId;
		WorkDate = workDate;
		HoursWorked = hoursWorked;
		Comment = comment;
    }

	public void UpdateHoursWorked(decimal hoursWorked)
	{
		if (hoursWorked < 0)
			throw new DomainException($"{nameof(hoursWorked)} cannot be negative.");
		HoursWorked = hoursWorked;
	}

    public void Update(decimal hours, string? comment)
    {
        if (hours < 0)
            throw new DomainException("Hours cannot be negative.");

        HoursWorked = hours;
        Comment = comment;
    }
}

public class PayrollRuleEntity : BaseEntity
{
	// Properties mapped from ERD
	public DateOnly EffectiveFrom { get; private set; }
	public decimal PremiumPercent { get; private set; }
	public string Description { get; private set; }

    public IReadOnlyCollection<ProductEntity> Products { get; private set; } = [];

    // Constructor
    public PayrollRuleEntity(DateOnly effectiveFrom, decimal premiumPercent, string description)
	{
		Guard.AgainstDefaultDate(effectiveFrom, nameof(effectiveFrom));
		if (premiumPercent < 0)
			throw new DomainException($"{nameof(premiumPercent)} cannot be negative.");
		Guard.AgainstNullOrWhiteSpace(description, nameof(description));

		EffectiveFrom = effectiveFrom;
		PremiumPercent = premiumPercent;
		Description = description;
	}

	public void Update(DateOnly effectiveFrom, decimal premiumPercent, string description)
	{
		Guard.AgainstDefaultDate(effectiveFrom, nameof(effectiveFrom));
		if (premiumPercent < 0)
			throw new DomainException($"{nameof(premiumPercent)} cannot be negative.");
		Guard.AgainstNullOrWhiteSpace(description, nameof(description));

		EffectiveFrom = effectiveFrom;
		PremiumPercent = premiumPercent;
		Description = description;
	}

	// No business methods specified in ERD/spec for this entity
}

public class PayrollAccrualEntity : AuditableEntity
{
	// Properties mapped from ERD
	public Guid EmployeeId { get; private set; }
	public DateOnly AccrualDate { get; private set; }
	public decimal HoursWorked { get; private set; }
	public decimal QtyPlanned { get; private set; }
	public decimal QtyProduced { get; private set; }
	public decimal QtyExtra { get; private set; }
    public decimal BaseAmount { get; private set; }
	public decimal PremiumAmount { get; private set; }
	public decimal TotalAmount { get; private set; }
	public string? AdjustmentReason { get; private set; }


	// Constructor
	public PayrollAccrualEntity(
		Guid employeeId,
		DateOnly accrualDate,
		decimal hoursWorked,
		decimal qtyPlanned,
		decimal qtyProduced,
		decimal qtyExtra,
		decimal baseAmount,
		decimal premiumAmount,
		decimal totalAmount,
		string? adjustmentReason = null)
	{
		Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));
		Guard.AgainstDefaultDate(accrualDate, nameof(accrualDate));
        if (hoursWorked < 0)
			throw new DomainException($"{nameof(hoursWorked)} cannot be negative.");
		if (qtyPlanned < 0)
			throw new DomainException($"{nameof(qtyPlanned)} cannot be negative.");
		if (qtyProduced < 0)
			throw new DomainException($"{nameof(qtyProduced)} cannot be negative.");
		if (qtyExtra < 0)
			throw new DomainException($"{nameof(qtyExtra)} cannot be negative.");
		if (baseAmount < 0)
			throw new DomainException($"{nameof(baseAmount)} cannot be negative.");
		if (premiumAmount < 0)
			throw new DomainException($"{nameof(premiumAmount)} cannot be negative.");
		if (totalAmount < 0)
			throw new DomainException($"{nameof(totalAmount)} cannot be negative.");

		EmployeeId = employeeId;
		AccrualDate = accrualDate;
		HoursWorked = hoursWorked;
		QtyPlanned = qtyPlanned;
		QtyProduced = qtyProduced;
		QtyExtra = qtyExtra;
        BaseAmount = baseAmount;
		PremiumAmount = premiumAmount;
		TotalAmount = totalAmount;
		AdjustmentReason = adjustmentReason;
	}

	public void Adjust(decimal baseAmount, decimal premiumAmount, string reason)
	{
		if (baseAmount < 0)
			throw new DomainException($"{nameof(baseAmount)} cannot be negative.");
		if (premiumAmount < 0)
			throw new DomainException($"{nameof(premiumAmount)} cannot be negative.");
		Guard.AgainstNullOrWhiteSpace(reason, nameof(reason));

		BaseAmount = baseAmount;
		PremiumAmount = premiumAmount;
		TotalAmount = baseAmount + premiumAmount;
		AdjustmentReason = reason;

		Touch();
	}
}

public class PayrollPaymentEntity : AuditableEntity
{
	// Properties mapped from ERD
	public Guid EmployeeId { get; private set; }
	public DateOnly PaymentDate { get; private set; }
	public decimal Amount { get; private set; }
	public Guid CreatedBy { get; private set; }

	// Constructor
	public PayrollPaymentEntity(Guid employeeId, DateOnly paymentDate, decimal amount, Guid createdBy)
	{
		Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));
		Guard.AgainstDefaultDate(paymentDate, nameof(paymentDate));
		if (amount < 0)
			throw new DomainException($"{nameof(amount)} cannot be negative.");
		Guard.AgainstEmptyGuid(createdBy, nameof(createdBy));

		EmployeeId = employeeId;
		PaymentDate = paymentDate;
		Amount = amount;
		CreatedBy = createdBy;
	}

	public void ChangeAmount(decimal amount)
	{
		if (amount < 0)
			throw new DomainException($"{nameof(amount)} cannot be negative.");
		Amount = amount;
	}
}

public class ExpenseEntity : AuditableEntity
{
	// Properties mapped from ERD
	public Guid ExpenseTypeId { get; private set; }
	public DateOnly ExpenseDate { get; private set; }
	public decimal Amount { get; private set; }
	public string? Description { get; private set; }
	public Guid CreatedBy { get; private set; }

	// Navigation property stubs (types must exist elsewhere in the domain layer)
	// public ExpenseTypeEntity? ExpenseType { get; private set; }
	// public UserEntity? Creator { get; private set; }

	// Constructor
	public ExpenseEntity(Guid expenseTypeId, DateOnly expenseDate, decimal amount, string? description, Guid createdBy)
	{
		Guard.AgainstEmptyGuid(expenseTypeId, nameof(expenseTypeId));
		Guard.AgainstDefaultDate(expenseDate, nameof(expenseDate));
		if (amount < 0)
			throw new DomainException($"{nameof(amount)} cannot be negative.");
		Guard.AgainstEmptyGuid(createdBy, nameof(createdBy));

		ExpenseTypeId = expenseTypeId;
		ExpenseDate = expenseDate;
		Amount = amount;
		Description = description;
		CreatedBy = createdBy;
	}

	// No business methods specified in ERD/spec for this entity
}

public class ExpenseTypeEntity : BaseEntity
{
	// Properties mapped from ERD
	public string Name { get; private set; }
	public string? Description { get; private set; }

	// Navigation property stubs (types must exist elsewhere in the domain layer)
	// public IReadOnlyCollection<ExpenseEntity> Expenses => _expenses;
	// private readonly List<ExpenseEntity> _expenses = new();

	// Constructor
	public ExpenseTypeEntity(string name, string? description)
	{
		Guard.AgainstNullOrWhiteSpace(name, nameof(name));
		Name = name;
		Description = description;
	}

	public void Update(string name, string? description)
	{
		Guard.AgainstNullOrWhiteSpace(name, nameof(name));
		Name = name;
		Description = description;
	}

	// No business methods specified in ERD/spec for this entity
}

public class CashAdvanceEntity : AuditableEntity
{
	// Properties mapped from ERD
	public Guid EmployeeId { get; private set; }
	public DateOnly IssueDate { get; private set; }
	public decimal Amount { get; private set; }

	// Navigation property stubs (types must exist elsewhere in the domain layer)
	// public EmployeeEntity? Employee { get; private set; }
	// public IReadOnlyCollection<CashAdvanceExpenseEntity> CashAdvanceExpenses => _cashAdvanceExpenses;
	// private readonly List<CashAdvanceExpenseEntity> _cashAdvanceExpenses = new();
	// public IReadOnlyCollection<CashAdvanceReturnEntity> CashAdvanceReturns => _cashAdvanceReturns;
	// private readonly List<CashAdvanceReturnEntity> _cashAdvanceReturns = new();

	// Constructor
	public CashAdvanceEntity(Guid employeeId, DateOnly issueDate, decimal amount)
	{
		Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));
		Guard.AgainstDefaultDate(issueDate, nameof(issueDate));
		if (amount < 0)
			throw new DomainException($"{nameof(amount)} cannot be negative.");

		EmployeeId = employeeId;
		IssueDate = issueDate;
		Amount = amount;
	}

	// No business methods specified in ERD/spec for this entity
}

public class CashAdvanceExpenseEntity : BaseEntity
{
	// Properties mapped from ERD
	public Guid CashAdvanceId { get; private set; }
	public DateOnly ExpenseDate { get; private set; }
	public decimal Amount { get; private set; }
	public string Description { get; private set; }

	// Navigation property stubs (types must exist elsewhere in the domain layer)
	// public CashAdvanceEntity? CashAdvance { get; private set; }

	// Constructor
	public CashAdvanceExpenseEntity(Guid cashAdvanceId, DateOnly expenseDate, decimal amount, string description)
	{
		Guard.AgainstEmptyGuid(cashAdvanceId, nameof(cashAdvanceId));
		Guard.AgainstDefaultDate(expenseDate, nameof(expenseDate));
		if (amount < 0)
			throw new DomainException($"{nameof(amount)} cannot be negative.");
		Guard.AgainstNullOrWhiteSpace(description, nameof(description));

		CashAdvanceId = cashAdvanceId;
		ExpenseDate = expenseDate;
		Amount = amount;
		Description = description;
	}

	// No business methods specified in ERD/spec for this entity
}

public class CashAdvanceReturnEntity : BaseEntity
{
	// Properties mapped from ERD
	public Guid CashAdvanceId { get; private set; }
	public DateOnly ReturnDate { get; private set; }
	public decimal Amount { get; private set; }
	public string Description { get; private set; }

	// Navigation property stubs (types must exist elsewhere in the domain layer)
	// public CashAdvanceEntity? CashAdvance { get; private set; }

	// Constructor
	public CashAdvanceReturnEntity(Guid cashAdvanceId, DateOnly returnDate, decimal amount, string description)
	{
		Guard.AgainstEmptyGuid(cashAdvanceId, nameof(cashAdvanceId));
		Guard.AgainstDefaultDate(returnDate, nameof(returnDate));
		if (amount < 0)
			throw new DomainException($"{nameof(amount)} cannot be negative.");
		Guard.AgainstNullOrWhiteSpace(description, nameof(description));

		CashAdvanceId = cashAdvanceId;
		ReturnDate = returnDate;
		Amount = amount;
		Description = description;
	}

	// No business methods specified in ERD/spec for this entity
}

public enum MonthlyReportStatus
{
	Draft = 0,
	Calculated = 1,
	Approved = 2,
	Closed = 3
}

public class MonthlyFinancialReportEntity : BaseEntity
{
	// Properties mapped from ERD
	public int ReportYear { get; private set; }
	public int ReportMonth { get; private set; }
	public decimal TotalRevenue { get; private set; }
	public decimal PayrollExpenses { get; private set; }
	public decimal MaterialExpenses { get; private set; }
	public decimal OtherExpenses { get; private set; }
	public MonthlyReportStatus Status { get; private set; }
	public DateTime CalculatedAt { get; private set; }
	public Guid CreatedBy { get; private set; }

	// Navigation property stubs (types must exist elsewhere in the domain layer)
	// public UserEntity? Creator { get; private set; }

	// Constructor
	public MonthlyFinancialReportEntity(
		int reportYear,
		int reportMonth,
		decimal totalRevenue,
		decimal payrollExpenses,
		decimal materialExpenses,
		decimal otherExpenses,
		MonthlyReportStatus status,
		DateTime calculatedAt,
		Guid createdBy)
	{
		if (reportYear < 2000 || reportYear > 2100)
			throw new DomainException($"{nameof(reportYear)} is out of range.");
		if (reportMonth < 1 || reportMonth > 12)
			throw new DomainException($"{nameof(reportMonth)} must be 1-12.");
		if (totalRevenue < 0)
			throw new DomainException($"{nameof(totalRevenue)} cannot be negative.");
		if (payrollExpenses < 0)
			throw new DomainException($"{nameof(payrollExpenses)} cannot be negative.");
		if (materialExpenses < 0)
			throw new DomainException($"{nameof(materialExpenses)} cannot be negative.");
		if (otherExpenses < 0)
			throw new DomainException($"{nameof(otherExpenses)} cannot be negative.");
		Guard.AgainstDefaultDate(calculatedAt.Date, nameof(calculatedAt));
		Guard.AgainstEmptyGuid(createdBy, nameof(createdBy));

		ReportYear = reportYear;
		ReportMonth = reportMonth;
		TotalRevenue = totalRevenue;
		PayrollExpenses = payrollExpenses;
		MaterialExpenses = materialExpenses;
		OtherExpenses = otherExpenses;
		Status = status;
		CalculatedAt = calculatedAt;
		CreatedBy = createdBy;
	}

	// No business methods specified in ERD/spec for this entity
}