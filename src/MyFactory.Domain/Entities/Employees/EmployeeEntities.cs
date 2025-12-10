using System;
using System.Collections.Generic;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Entities.Shifts;
using MyFactory.Domain.Enums;

namespace MyFactory.Domain.Entities.Employees;

public sealed class Employee : BaseEntity
{
    private readonly List<TimesheetEntry> _timesheetEntries = new();
    private readonly List<PayrollEntry> _payrollEntries = new();
    private readonly List<ShiftPlan> _shiftPlans = new();
    private readonly List<WorkerAssignment> _workerAssignments = new();

    private Employee()
    {
    }

    public Employee(string fullName, string position, int grade, decimal ratePerNormHour, decimal premiumPercent)
    {
        UpdateName(fullName);
        UpdatePosition(position);
        UpdateGrade(grade);
        UpdateRate(ratePerNormHour);
        UpdatePremium(premiumPercent);
        IsActive = true;
    }

    public string FullName { get; private set; } = string.Empty;

    public string Position { get; private set; } = string.Empty;

    public int Grade { get; private set; }

    public decimal RatePerNormHour { get; private set; }

    public decimal PremiumPercent { get; private set; }

    public bool IsActive { get; private set; }

    public IReadOnlyCollection<TimesheetEntry> TimesheetEntries => _timesheetEntries.AsReadOnly();

    public IReadOnlyCollection<PayrollEntry> PayrollEntries => _payrollEntries.AsReadOnly();

    public IReadOnlyCollection<ShiftPlan> ShiftPlans => _shiftPlans.AsReadOnly();

    public IReadOnlyCollection<WorkerAssignment> WorkerAssignments => _workerAssignments.AsReadOnly();

    public void UpdateName(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Employee name is required.");
        FullName = name.Trim();
    }

    public void UpdatePosition(string position)
    {
        Guard.AgainstNullOrWhiteSpace(position, "Position is required.");
        Position = position.Trim();
    }

    public void UpdateGrade(int grade)
    {
        if (grade <= 0)
        {
            throw new DomainException("Grade must be positive.");
        }

        Grade = grade;
    }

    public void UpdateRate(decimal rate)
    {
        Guard.AgainstNegative(rate, "Rate per norm hour cannot be negative.");
        RatePerNormHour = rate;
    }

    public void UpdatePremium(decimal percent)
    {
        Guard.AgainstNegative(percent, "Premium percent cannot be negative.");
        PremiumPercent = percent;
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new DomainException("Employee already inactive.");
        }

        IsActive = false;
    }
}

public sealed class TimesheetEntry : BaseEntity
{
    private TimesheetEntry()
    {
    }

    private TimesheetEntry(Guid employeeId, DateOnly workDate, decimal hoursWorked, Guid? productionOrderId)
    {
        Guard.AgainstEmptyGuid(employeeId, "Employee id is required.");
        Guard.AgainstDefaultDate(workDate, "Work date is required.");
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (workDate > today)
        {
            throw new DomainException("Work date cannot be in the future.");
        }

        EmployeeId = employeeId;
        WorkDate = workDate;
        Status = TimesheetStatus.Draft;
        UpdateHours(hoursWorked);
        AssignProductionOrder(productionOrderId);
    }

    public static TimesheetEntry Create(Guid employeeId, DateOnly workDate, decimal hoursWorked, Guid? productionOrderId)
    {
        return new TimesheetEntry(employeeId, workDate, hoursWorked, productionOrderId);
    }

    public Guid EmployeeId { get; private set; }

    public Employee? Employee { get; private set; }

    public DateOnly WorkDate { get; private set; }

    public decimal HoursWorked { get; private set; }

    public Guid? ProductionOrderId { get; private set; }

    public ProductionOrder? ProductionOrder { get; private set; }

    public TimesheetStatus Status { get; private set; } = TimesheetStatus.Draft;

    public void UpdateHours(decimal hoursWorked)
    {
        EnsureDraftState();
        Guard.AgainstNegative(hoursWorked, "Hours worked cannot be negative.");
        HoursWorked = hoursWorked;
    }

    public void AssignProductionOrder(Guid? productionOrderId)
    {
        EnsureDraftState();
        ProductionOrderId = NormalizeProductionOrderId(productionOrderId);
        if (!ProductionOrderId.HasValue)
        {
            ProductionOrder = null;
        }
    }

    internal void LinkToProductionOrder(ProductionOrder? productionOrder)
    {
        EnsureDraftState();
        if (productionOrder is null)
        {
            ProductionOrderId = null;
            ProductionOrder = null;
            return;
        }

        ProductionOrderId = productionOrder.Id;
        ProductionOrder = productionOrder;
    }

    public void Approve()
    {
        if (Status == TimesheetStatus.Approved)
        {
            throw new DomainException("Timesheet entry is already approved.");
        }

        Status = TimesheetStatus.Approved;
    }

    public void Reject()
    {
        if (Status == TimesheetStatus.Rejected)
        {
            throw new DomainException("Timesheet entry already rejected.");
        }

        if (Status == TimesheetStatus.Draft)
        {
            throw new DomainException("Draft entries cannot be rejected without submission.");
        }

        Status = TimesheetStatus.Rejected;
    }

    public void ReturnToDraft()
    {
        if (Status == TimesheetStatus.Draft)
        {
            throw new DomainException("Entry already in draft state.");
        }

        Status = TimesheetStatus.Draft;
    }

    private static Guid? NormalizeProductionOrderId(Guid? productionOrderId)
    {
        if (!productionOrderId.HasValue || productionOrderId == Guid.Empty)
        {
            return null;
        }

        return productionOrderId;
    }

    private void EnsureDraftState()
    {
        if (Status != TimesheetStatus.Draft)
        {
            throw new DomainException("Approved timesheet entries cannot be modified.");
        }
    }
}

public sealed class PayrollEntry : BaseEntity
{
    private PayrollEntry()
    {
    }

    private PayrollEntry(Guid employeeId, DateOnly periodStart, DateOnly periodEnd, decimal totalHours, decimal accruedAmount)
    {
        Guard.AgainstEmptyGuid(employeeId, "Employee id is required.");
        Guard.AgainstDefaultDate(periodStart, "Period start is required.");
        Guard.AgainstDefaultDate(periodEnd, "Period end is required.");
        if (periodEnd < periodStart)
        {
            throw new DomainException("Period end cannot be before period start.");
        }

        EmployeeId = employeeId;
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
        UpdateTotals(totalHours, accruedAmount);
    }

    public static PayrollEntry Create(Guid employeeId, DateOnly periodStart, DateOnly periodEnd, decimal totalHours, decimal accruedAmount)
    {
        return new PayrollEntry(employeeId, periodStart, periodEnd, totalHours, accruedAmount);
    }

    public Guid EmployeeId { get; private set; }

    public Employee? Employee { get; private set; }

    public DateOnly PeriodStart { get; private set; }

    public DateOnly PeriodEnd { get; private set; }

    public decimal TotalHours { get; private set; }

    public decimal AccruedAmount { get; private set; }

    public decimal PaidAmount { get; private set; }

    public decimal Outstanding { get; private set; }

    public void UpdateTotals(decimal totalHours, decimal accruedAmount)
    {
        Guard.AgainstNegative(totalHours, "Total hours cannot be negative.");
        Guard.AgainstNegative(accruedAmount, "Accrued amount cannot be negative.");
        if (accruedAmount < PaidAmount)
        {
            throw new DomainException("Accrued amount cannot be less than already paid amount.");
        }

        TotalHours = totalHours;
        AccruedAmount = accruedAmount;
        RecalculateOutstanding();
    }

    public void AddPayment(decimal amount)
    {
        Guard.AgainstNonPositive(amount, "Payment amount must be positive.");
        if (amount > Outstanding)
        {
            throw new DomainException("Payment exceeds outstanding amount.");
        }

        PaidAmount += amount;
        RecalculateOutstanding();
    }

    public void RecalculateOutstanding()
    {
        Outstanding = AccruedAmount - PaidAmount;
        if (Outstanding < 0)
        {
            throw new DomainException("Outstanding amount cannot be negative.");
        }
    }
}

